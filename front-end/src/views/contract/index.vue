<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input :placeholder="$t('table.teamName')" v-model="listQuery.teamName" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <!-- <el-select v-model="listQuery.sort" style="width: 160px" class="filter-item" @change="handleFilter">
        <el-option v-for="item in sortOptions" :key="item.key" :label="item.label" :value="item.key"/>
      </el-select> -->
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <el-button v-show="userPermission" class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">{{ $t('table.add') }}</el-button>
      <el-button v-waves :loading="downloadLoading" class="filter-item" type="primary" icon="el-icon-download" @click="handleDownload">{{ $t('table.export') }}</el-button>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-refresh" @click="getList">{{ $t('table.refresh') }}</el-button>
    </div>

    <el-table
      v-loading="listLoading"
      :key="tableKey"
      :data="list"
      border
      fit
      stripe
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column :label="$t('contract.contractName')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.contractName }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('contract.user')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.user[0].fullName }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('contract.team')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.team[0].teamName }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('contract.position')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.position[0].positionName }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('contract.startDate')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.startDate | parseTime('{d}-{m}-{y}') }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('contract.endDate')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.endDate | parseTime('{d}-{m}-{y}') }}</div>
        </template>
      </el-table-column>
      <el-table-column v-if="userPermission" :label="$t('table.actions')" align="center" class-name="small-padding fixed-width" width="150px">
        <template slot-scope="scope">
          <el-button type="primary" size="medium" icon="el-icon-edit" @click="handleUpdate(scope.row)"/>
          <el-button
            v-if="scope.row.status!='deleted'"
            size="medium"
            type="danger"
            icon="el-icon-delete"
            @click="handleModifyStatus(scope.row,'deleted')"/>
        </template>
      </el-table-column>
    </el-table>

    <div class="pagination-container">
      <el-pagination v-show="total>0" :current-page="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.limit" :total="total" background layout="total, sizes, prev, pager, next, jumper" @size-change="handleSizeChange" @current-change="handleCurrentChange"/>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible" width="70%">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="160px">
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item :label="$t('contract.contractName')" prop="contractName">
              <el-input v-model="temp.contractName" style="width: 100%"/>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('contract.user')" prop="userId">
              <el-select
                v-model="temp.userId"
                :disabled="dialogStatus=='update'"
                class="filter-item"
                placeholder="Please select"
                style="width: 100%">
                <el-option v-for="item in lstUser" :key="item.userId" :label="item.fullName" :value="item.userId"/>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item :label="$t('contract.team')" prop="teamId">
              <el-select
                v-model="temp.teamId"
                filterable
                allow-create
                class="filter-item"
                placeholder="Please select"
                style="width: 100%">
                <el-option v-for="item in lstTeam" :key="item.teamId" :label="item.teamName" :value="item.teamId"/>
                <li class="el-select-dropdown__item" style="color: red" @click="openCreateTeam">
                  Tạo đội
                </li>
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('contract.position')" prop="positionId">
              <el-select
                v-model="temp.positionId"
                filterable
                allow-create
                class="filter-item"
                placeholder="Please select"
                style="width: 100%">
                <el-option v-for="item in lstPosition" :key="item.positionId" :label="item.positionName" :value="item.positionId"/>
                <li class="el-select-dropdown__item" style="color: red" @click="openCreatePosition">
                  Tạo vị trí
                </li>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item :label="$t('contract.startDate')" prop="startDate">
              <el-date-picker
                v-model="temp.startDate"
                :placeholder="$t('i18nView.datePlaceholder')"
                :disabled="dialogStatus=='update'"
                type="datetime"
                format="MM/dd/yyyy"
                style="width: 100%"/>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('contract.endDate')" prop="endDate">
              <el-date-picker
                v-model="temp.endDate"
                :placeholder="$t('i18nView.datePlaceholder')"
                type="datetime"
                format="MM/dd/yyyy"
                style="width: 100%"/>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item :label="$t('contract.disableDate')" prop="disableDate">
              <el-date-picker
                v-model="temp.disableDate"
                :placeholder="$t('i18nView.datePlaceholder')"
                type="datetime"
                format="MM/dd/yyyy"
                style="width: 100%"/>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('contract.monthlyNetSalary')" prop="monthlyNetSalary">
              <el-input-number v-model="temp.monthlyNetSalary" :min="0" :step="100" style="width: 100%"/>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item :label="$t('contract.hourlyNetSalary')" prop="hourlyNetSalary">
              <el-input-number v-model="temp.hourlyNetSalary" :min="0" :step="100" style="width: 100%"/>
            </el-form-item>
          </el-col>
          <el-col :span="12">
            <el-form-item :label="$t('contract.extraBonus')" prop="extraBonus">
              <el-input-number v-model="temp.extraBonus" :min="0" :step="100" style="width: 100%"/>
          </el-form-item></el-col>
        </el-row>
        <el-row :gutter="20">
          <el-col :span="12">
            <el-form-item :label="$t('contract.officialEmployee')" prop="officialEmployee">
              <el-checkbox v-model="temp.officialEmployee"/>
            </el-form-item>
          </el-col>
          <el-col v-if="dialogStatus=='update'" :span="12">
            <el-form-item :label="$t('contract.active')" prop="active">
              <el-checkbox v-model="temp.active"/>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="createData">{{ $t('table.confirm') }}</el-button>
        <el-button v-else type="primary" @click="updateData">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <confirm-dialog :confirm="confirm" :data="temp.contractId" :call-back="handleDelete" :on-close="()=>confirm = false"/>
    <add-position ref="addPosition"/>
    <add-team ref="addTeam"/>

  </div>
</template>

<script>
import waves from '@/directive/waves' // 水波纹指令
import { parseTime, compareValues } from '@/utils'
import { mapGetters } from 'vuex'
import ConfirmDialog from '@/components/ConfirmDialog'
import AddPosition from './component/addPosition/addPosition'
import AddTeam from './component/AddTeam/addTeam'

export default {
  name: 'Contract',
  directives: {
    waves
  },
  components: { ConfirmDialog, AddPosition, AddTeam },
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'info',
        deleted: 'danger'
      }
      return statusMap[status]
    }
  },
  data() {
    return {
      tableKey: 0,
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        limit: 10,
        // importance: undefined,
        contractName: undefined,
        // type: undefined,
        sort: '+createdDate'
      },
      sortOptions: [{ label: 'Date Ascending', key: '+createdDate' }, { label: 'Date Descending', key: '-createdDate' }],
      temp: {
        contractId: null,
        contractName: null,
        userId: null,
        teamId: null,
        active: null,
        positionId: null,
        monthlyNetSalary: null,
        hourlyNetSalary: null,
        officialEmployee: null,
        extraBonus: null,
        startDate: null,
        endDate: null,
        disableDate: null,
      },
      dialogFormVisible: false,
      dialogExcUser: false,
      dialogStatus: '',
      textMap: {
        update: 'Edit',
        create: 'Create',
      },
      rules: {
        contractName: [{ required: true, message: 'contract Name is required', trigger: 'blur' }],
        userId: [{ required: true, message: 'User is required', trigger: 'blur' }],
        teamId: [{ required: true, message: 'Team is required', trigger: 'blur' }],
        positionId: [{ required: true, message: 'Position is required', trigger: 'blur' }],
        startDate: [{ required: true, message: 'Start Date is required', trigger: 'blur' }],
        endDate: [{ required: true, message: 'End Date is required', trigger: 'blur' }],
      },
      downloadLoading: false,
      confirm: false,
      lstPosition: [],
      lstTeam: [],
      lstUser: [],
    }
  },
  computed: {
    ...mapGetters([
      'curUser',
      'userPermission',
    ])
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.$store.dispatch('GetAllContract').then(res => {
        if (res) {
          this.list = res.data.sort(compareValues(this.listQuery.sort))
        }
      }).then(() => {
        this.getListTeam()
      }).then(() => {
        this.getListUser()
      }).then(() => {
        this.getListPosition()
        this.listLoading = false
      })
        .catch(e => {
          console.log(e);
          this.$notify({
            title: 'Lỗi',
            message: 'getList ' + JSON.stringify(e),
            type: 'error',
            duration: 2000
          })
        })
    },
    getListTeam() {
      this.$store.dispatch('GetAllTeam').then(res => {
        if (res) {
          this.lstTeam = res;
        }
      }).catch(e => {
        console.log(e);
      })
    },
    getListUser() {
      this.$store.dispatch('GetAllUser', { q: null, role: null, available: null }).then(res => {
        if (res) {
          this.lstUser = res;
        }
      }).catch(e => {
        console.log(e);
      })
    },
    getListPosition() {
      this.$store.dispatch('GetAllPosition').then(res => {
        if (res) {
          this.lstPosition = res;
        }
      }).catch(e => {
        console.log(e);
      })
    },
    handleFilter() {
      this.listQuery.page = 1
      this.getList()
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      this.getList()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.getList()
    },
    handleModifyStatus(row, status) {
      this.confirm = true;
      this.temp = Object.assign({}, row)
    },
    handleDelete() {
      this.$store.dispatch('DeleteContract', this.temp.contractId).then(res => {
        if (res) {
          this.dialogFormVisible = false
          this.confirm = false
          this.$notify({
            title: 'Success',
            message: 'Delete successfully',
            type: 'success',
            duration: 2000
          })
        }
        this.getList()
      }).catch(e => {
        this.$notify({
          title: 'Error',
          message: 'Delete unsuccessfully ' + JSON.stringify(e),
          type: 'error',
          duration: 2000
        })
      });
    },
    resetTemp() {
      this.temp = {
        contractId: null,
        contractName: null,
        userId: null,
        teamId: null,
        active: null,
        positionId: null,
        monthlyNetSalary: null,
        hourlyNetSalary: null,
        officialEmployee: null,
        extraBonus: null,
        startDate: null,
        endDate: null,
        disableDate: null,
      }
    },
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.$store.dispatch('CreateContract', this.temp).then(res => {
            this.dialogFormVisible = false
            if (res) {
              this.$notify({
                title: 'Thành công',
                message: 'Tạo thành công',
                type: 'success',
                duration: 2000
              })
              this.getList()
            }
          });
        }
      })
    },
    handleUpdate(row) {
      this.temp = Object.assign({}, row) // copy obj
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.$store.dispatch('UpdateContract', { contractId: this.temp.contractId, newContract: this.temp }).then(res => {
            if (res) {
              this.dialogFormVisible = false
              this.$notify({
                title: 'Thành công',
                message: 'Sửa thành công',
                type: 'success',
                duration: 2000
              })
              this.getList()
            }
          });
        }
      })
    },
    handleDownload() {
      this.downloadLoading = true
      import('@/vendor/Export2Excel').then(excel => {
        const tHeader = Object.keys(this.temp);
        const filterVal = Object.keys(this.temp);
        const data = this.formatJson(filterVal, this.list)
        excel.export_json_to_excel({
          header: tHeader,
          data,
          filename: 'list team'
        })
        this.downloadLoading = false
      })
    },
    formatJson(filterVal, jsonData) {
      return jsonData.map(v => filterVal.map(j => {
        if (j === 'createdDate' || j === 'lastModifyDate') {
          return parseTime(v[j])
        } else {
          return v[j]
        }
      }))
    },
    openCreateTeam() {
      this.$refs['addTeam'].handleOpen()
    },
    openCreatePosition() {
      this.$refs['addPosition'].handleOpen()
    },
  }
}
</script>
<style lang="scss">
  .mt-15{
    margin-top: 15px;
  }
  h4 {
    font-weight: bold;
  }
  .icon-fs{
    font-size: 3rem;
  }
  .border{
    border: 1px solid;
    border-radius: 5%;
  }
  .list-group-item{
    cursor: pointer;
  }
  .list-group-item.active,
  .list-group-item.active:hover{
    background-color: #C9FFE5;
    border-color: #C9FFE5;
    color: black;
  }
</style>
