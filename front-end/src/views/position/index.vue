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
      <el-table-column :label="$t('position.positionName')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.positionName }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('position.baseDateOff')" align="center" prop="baseDateOff">
        <template slot-scope="scope" >
          <div>{{ scope.row.baseDateOff }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('position.baseMonthSalary')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.baseMonthSalary }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('position.baseHourSalary')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.baseHourSalary }}</div>
        </template>
      </el-table-column>
      <el-table-column v-if="userPermission" :label="$t('table.actions')" align="center" class-name="small-padding fixed-width" >
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

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="150px" style="width: 70%; margin-left:50px;">
        <el-form-item :label="$t('position.positionName')" prop="positionName">
          <el-input v-model="temp.positionName"/>
        </el-form-item>
        <el-form-item :label="$t('position.description')" prop="description">
          <el-input v-model="temp.description"/>
        </el-form-item>
        <el-form-item :label="$t('position.baseMonthSalary')" prop="baseMonthSalary">
          <el-input-number v-model="temp.baseMonthSalary" :min="0" :step="100"/>
        </el-form-item>
        <el-form-item :label="$t('position.baseHourSalary')" prop="baseHourSalary">
          <el-input-number v-model="temp.baseHourSalary" :min="0" :step="100"/>
        </el-form-item>
        <el-form-item :label="$t('position.baseOtSalaryPerHour')" prop="baseOtSalaryPerHour">
          <el-input-number v-model="temp.baseOtSalaryPerHour" :min="0" :step="100"/>
        </el-form-item>
        <el-form-item :label="$t('position.baseDateOff')" prop="baseDateOff">
          <el-input-number v-model="temp.baseDateOff" :min="0" :step="100"/>
        </el-form-item>
        <el-form-item :label="$t('position.baseLateMoney')" prop="baseLateMoney">
          <el-input-number v-model="temp.baseLateMoney" :min="0" :step="100"/>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="createData">{{ $t('table.confirm') }}</el-button>
        <el-button v-else type="primary" @click="updateData">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <confirm-dialog :confirm="confirm" :data="temp.positionId" :call-back="handleDelete" :on-close="()=>confirm = false"/>

  </div>
</template>

<script>
import waves from '@/directive/waves' // 水波纹指令
import { parseTime, compareValues } from '@/utils'
import { mapGetters } from 'vuex'
import ConfirmDialog from '@/components/ConfirmDialog'

export default {
  name: 'Position',
  directives: {
    waves
  },
  components: { ConfirmDialog },
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
        positionName: undefined,
        // type: undefined,
        sort: '+createdDate'
      },
      sortOptions: [{ label: 'Date Ascending', key: '+createdDate' }, { label: 'Date Descending', key: '-createdDate' }],
      temp: {
        positionId: undefined,
        positionName: '',
        description: '',
        baseMonthSalary: 0,
        baseHourSalary: 0,
        baseOtSalaryPerHour: 0,
        baseDateOff: 0,
        baseLateMoney: 0,
      },
      dialogFormVisible: false,
      dialogExcUser: false,
      dialogStatus: '',
      textMap: {
        update: 'Edit',
        create: 'Create',
      },
      rules: {
        positionName: [{ required: true, message: 'Position Name is required', trigger: 'blur' }],
        baseMonthSalary: [{ required: true, message: 'Base Month Salary is required', trigger: 'blur' }],
        baseHourSalary: [{ required: true, message: 'Base Hour Salary is required', trigger: 'blur' }],
        baseOtSalaryPerHour: [{ required: true, message: 'Base OT Salary per hour is required', trigger: 'blur' }],
        baseDateOff: [{ required: true, message: 'Base Date off is required', trigger: 'blur' }],
        baseLateMoney: [{ required: true, message: 'Base Late Money is required', trigger: 'blur' }],
      },
      downloadLoading: false,
      confirm: false
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
      this.$store.dispatch('GetAllPosition').then(res => {
        if (res) {
          this.list = res.sort(compareValues(this.listQuery.sort));
          this.listLoading = false;
        }
      }).catch(e => {
        this.$notify({
          title: 'Error',
          message: 'getList ' + JSON.stringify(e),
          type: 'error',
          duration: 2000
        })
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
      // this.$store.dispatch('DeleteTeam', this.temp.teamId).then(res => {
      //   if (res) {
      //     this.dialogFormVisible = false
      //     this.confirm = false
      //     this.$notify({
      //       title: 'Success',
      //       message: 'Delete successfully',
      //       type: 'success',
      //       duration: 2000
      //     })
      //   }
      //   this.getList()
      // }).catch(e => {
      //   this.$notify({
      //     title: 'Error',
      //     message: 'Delete unsuccessfully ' + JSON.stringify(e),
      //     type: 'error',
      //     duration: 2000
      //   })
      // });
    },
    resetTemp() {
      this.temp = {
        positionId: undefined,
        positionName: '',
        description: '',
        baseMonthSalary: 0,
        baseHourSalary: 0,
        baseOtSalaryPerHour: 0,
        baseDateOff: 0,
        baseLateMoney: 0,
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
          this.$store.dispatch('CreatePosition', this.temp).then(res => {
            this.dialogFormVisible = false
            this.$notify({
              title: 'Thành công',
              message: 'Tạo thành công',
              type: 'success',
              duration: 2000
            })
            this.getList()
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
          this.$store.dispatch('UpdatePosition', { positionId: this.temp.positionId, newPosition: this.temp }).then(res => {
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
    }
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
