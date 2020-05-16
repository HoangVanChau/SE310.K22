<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input :placeholder="$t('table.fullName')" v-model="listQuery.name" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.sort" style="width: 160px" class="filter-item" @change="handleFilter">
        <el-option v-for="item in sortOptions" :key="item.key" :label="item.label" :value="item.key"/>
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">{{ $t('table.add') }}</el-button>
      <el-button v-waves :loading="downloadLoading" class="filter-item" type="primary" icon="el-icon-download" @click="handleDownload">{{ $t('table.export') }}</el-button>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-refresh" @click="getList">{{ $t('table.refresh') }}</el-button>
    </div>
    <!-- :default-sort = "{prop: 'date', order: 'descending'}" -->

    <el-table
      v-loading="listLoading"
      :key="tableKey"
      :data="list"
      border
      fit
      stripe
      highlight-current-row
      style="width: 100%;">
      <!-- <el-table-column :label="$t('table.id')" :sort-method="handleFilter" align="center" hidden="hidden" prop="userId" >
        <template slot-scope="scope" style="visible: hidden">
          <span>{{ scope.row.userId }}</span>
        </template>
      </el-table-column> -->
      <el-table-column :label="$t('table.fullName')" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.fullName }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.createdDate')" align="center" sortable prop="createdDate">
        <template slot-scope="scope">
          <span>{{ scope.row.createdDate | parseTime('{d}-{m}-{y}') }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.lastModifiedDate')" align="center" sortable prop="lastModifiedDate">
        <template slot-scope="scope">
          <span>{{ scope.row.lastModifyDate | parseTime('{d}-{m}-{y}') }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.dateOfBirth')" align="center" sortable prop="dateOfbirth">
        <template slot-scope="scope">
          <span>{{ scope.row.dateOfBirth | parseTime('{d}-{m}-{y}') }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.phoneNumber')" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.phoneNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.role')" align="center" sortable prop="role">
        <template slot-scope="scope">
          <span>{{ scope.row.role }}</span>
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
        <el-form-item :label="$t('table.fullName')" prop="fullName">
          <el-input v-model="temp.fullName"/>
        </el-form-item>
        <el-form-item :label="$t('login.username')" prop="userName">
          <el-input v-model="temp.userName"/>
        </el-form-item>
        <el-form-item :label="'Email'" prop="email">
          <el-input v-model="temp.email"/>
        </el-form-item>
        <el-form-item :label="$t('table.phoneNumber')" prop="phoneNumber">
          <el-input v-model="temp.phoneNumber"/>
        </el-form-item>
        <el-form-item :label="$t('table.dateOfBirth')" prop="dateOfBirth">
          <el-date-picker
            v-model="temp.dateOfBirth"
            :placeholder="$t('i18nView.datePlaceholder')"
            type="date"
            format="MM/dd/yyyy"
            style="width: 100%"/>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-if="dialogStatus=='update'" type="success" @click="handleChangeRole(temp)">{{ $t('table.changeRole') }}</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="createData">{{ $t('table.confirm') }}</el-button>
        <el-button v-else type="primary" @click="updateData">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <confirm-dialog :confirm="confirm" :data="temp.fullName" :call-back="handleDelete" :on-close="()=>confirm = false"/>

    <!-- <el-dialog :visible.sync="confirm" title="Xác nhận">
      <div class="" style="width: 70%; margin-left: 50px;">
        {{ $t('confirm.deleteMes') }}
        {{ temp.fullName }}
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="confirm = false">{{ $t('table.cancel') }}</el-button>
        <el-button type="primary" @click="handleDelete">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog> -->

    <el-dialog :visible.sync="openChangeRole" title="Change Role">
      <el-form :rules="rules" :model="temp" label-position="left" label-width="150px" style="width: 70%; margin-left:50px;">
        <el-form-item :label="$t('table.role')" prop="role">
          <el-select v-model="temp.role" class="filter-item" placeholder="Please select" style="width: 100%">
            <el-option v-for="item in roles" :key="item" :label="item" :value="item"/>
          </el-select>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="openChangeRole = false">{{ $t('table.cancel') }}</el-button>
        <el-button type="primary" @click="changeRole">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

  </div>
</template>

<script>
import waves from '@/directive/waves' // 水波纹指令
import { parseTime, compareValues } from '@/utils'
import ConfirmDialog from '@/components/ConfirmDialog'
import { mapGetters } from 'vuex'

export default {
  name: 'Employee',
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
      list: [],
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        limit: 10,
        // importance: undefined,
        name: null,
        role: null,
        available: null,
        sort: '+createdDate'
      },
      sortOptions: [{ label: 'Date Ascending', key: '+createdDate' }, { label: 'Date Descending', key: '-createdDate' }],
      temp: {
        userId: undefined,
        employeeId: undefined,
        fullName: '',
        userName: '',
        email: '',
        avatarImageId: '',
        phoneNumber: '',
        address: '',
        dateOfBirth: '',
        lastModifyDate: Date.now(),
        role: 'User'
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: 'Edit',
        create: 'Create'
      },
      rules: {
        fullName: [{ required: true, message: 'Full name is required', trigger: 'change' }],
        timestamp: [{ type: 'date', required: true, message: 'timestamp is required', trigger: 'change' }],
        userName: [{ required: true, message: 'User Name is required', trigger: 'blur' }],
        email: [{ required: true, message: 'Email is required', trigger: 'blur' }],
        phoneNumber: [{ required: true, message: 'Phone Number is required', trigger: 'blur' }],
        dateOfBirth: [{ required: true, message: 'Date of birth is required', trigger: 'blur' }],
        role: [{ required: true, message: 'Role is required', trigger: 'blur' }]

      },
      downloadLoading: false,
      roles: [],
      confirm: false,
      openChangeRole: false
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
    this.getRoles()
  },
  methods: {
    getList() {
      this.listLoading = true
      this.$store.dispatch('GetAllUser', this.listQuery).then(items => {
        const begin = (this.listQuery.page - 1) * this.listQuery.limit;
        const end = begin + this.listQuery.limit;
        this.list = items.map(item => {
          this.$set(item, 'edit', false) // https://vuejs.org/v2/guide/reactivity.html will be used when user click the cancel botton
          return item
        })
          .sort(compareValues(this.listQuery.sort))
          .slice(begin, end)
        this.total = items.length;
        this.listLoading = false;
      });
    },
    getRoles() {
      this.$store.dispatch('GetAllRole').then((result) => {
        this.roles = result;
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
      // this.$message({
      //   message: 'Success',
      //   type: 'success'
      // })
      // row.status = status
      this.confirm = true;
      this.temp = Object.assign({}, row)
    },
    resetTemp() {
      this.temp = {
        serId: undefined,
        employeeId: undefined,
        fullName: '',
        userName: '',
        email: '',
        avatarImageId: '',
        phoneNumber: '',
        address: '',
        dateOfBirth: '',
        lastModifyDate: Date.now(),
        role: 'User'
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
          this.$store.dispatch('CreateUser', this.temp).then(res => {
            if (res) {
              this.dialogFormVisible = false
              this.$notify({
                title: 'Success',
                message: 'Create successfully',
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
          const tempData = Object.assign({}, this.temp)
          this.$store.dispatch('UpdateUser', { userId: tempData.userId, newData: tempData }).then(res => {
            if (res) {
              this.dialogFormVisible = false
              this.$notify({
                title: 'Success',
                message: 'Update successfully',
                type: 'success',
                duration: 2000
              })
              this.getList()
            }
          });
        }
      })
    },
    handleDelete() {
      this.$store.dispatch('DeleteUser', this.temp.userId).then(res => {
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
    handleDownload() {
      this.downloadLoading = true
      import('@/vendor/Export2Excel').then(excel => {
        const tHeader = ['id', 'fullName', 'createdDate', 'lastModifyDate', 'dateOfBirth', 'email', 'phoneNumber', 'role']
        const filterVal = ['id', 'fullName', 'createdDate', 'lastModifyDate', 'dateOfBirth', 'email', 'phoneNumber', 'role']
        const data = this.formatJson(filterVal, this.list)
        excel.export_json_to_excel({
          header: tHeader,
          data,
          filename: 'ListUser'
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
    changeRole() {
      const data = {
        userId: this.temp.userId,
        newRole: this.temp.role
      }
      this.$store.dispatch('ChangeRole', data).then(res => {
        if (res) {
          this.openChangeRole = false
          this.$notify({
            title: 'Success',
            message: 'Change Role successfully',
            type: 'success',
            duration: 2000
          })
          this.getList()
        }
      }).catch(e => {
        this.$notify({
          title: 'Error',
          message: 'Change Role unsuccessfully ' + JSON.stringify(e),
          type: 'error',
          duration: 2000
        })
      });
    },
    handleChangeRole(data) {
      this.openChangeRole = true;
    }
  }
}
</script>

