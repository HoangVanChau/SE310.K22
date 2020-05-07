<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input :placeholder="$t('table.title')" v-model="listQuery.title" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.sort" style="width: 140px" class="filter-item" @change="handleFilter">
        <el-option v-for="item in sortOptions" :key="item.key" :label="item.label" :value="item.key"/>
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">{{ $t('table.add') }}</el-button>
      <el-button v-waves :loading="downloadLoading" class="filter-item" type="primary" icon="el-icon-download" @click="handleDownload">{{ $t('table.export') }}</el-button>
    </div>

    <el-table
      v-loading="listLoading"
      :key="tableKey"
      :data="list"
      border
      fit
      stripe
      highlight-current-row
      style="width: 100%;">
      <el-table-column :label="$t('table.id')" align="center" hidden="true">
        <template slot-scope="scope">
          <span>{{ scope.row.teamId }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.leader')" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.leaders[0].fullName }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.createdDate')" align="center" sortable prop="createdDate">
        <template slot-scope="scope">
          <span>{{ scope.row.createdDate | parseTime('{d}-{m}-{y}') }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.lastModifiedDate')" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.lastModifyDate | parseTime('{d}-{m}-{y}') }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.teamName')" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.teamName }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" width="230" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button type="primary" size="medium" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="medium" type="danger" @click="handleModifyStatus(scope.row,'deleted')">{{ $t('table.delete') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <div class="pagination-container">
      <el-pagination v-show="total>0" :current-page="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.limit" :total="total" background layout="total, sizes, prev, pager, next, jumper" @size-change="handleSizeChange" @current-change="handleCurrentChange"/>
    </div>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="150px" style="width: 70%; margin-left:50px;">
        <el-form-item :label="$t('table.leader')" prop="leaderId">
          <el-select v-model="temp.leaderId" class="filter-item" placeholder="Please select" style="width: 100%">
            <el-option v-for="item in lstLeader" :key="item.userId" :label="item.fullName" :value="item.userId"/>
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.teamName')" prop="teamName">
          <el-input v-model="temp.teamName"/>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-if="dialogStatus=='create'" type="primary" @click="createData">{{ $t('table.confirm') }}</el-button>
        <el-button v-else type="primary" @click="updateData">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

  </div>
</template>

<script>
import waves from '@/directive/waves' // 水波纹指令
import { parseTime, compareValues } from '@/utils'

export default {
  name: 'Role',
  directives: {
    waves
  },
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
        limit: 20,
        // importance: undefined,
        teamName: undefined,
        // type: undefined,
        sort: '+createdDate'
      },
      sortOptions: [{ label: 'Date Ascending', key: '+createdDate' }, { label: 'Date Descending', key: '-createdDate' }],
      temp: {
        id: undefined,
        teamName: '',
        leaderId: '',
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: 'Edit',
        create: 'Create'
      },
      rules: {
        leaderId: [{ required: true, message: 'leader is required', trigger: 'change' }],
        timestamp: [{ type: 'date', required: true, message: 'timestamp is required', trigger: 'change' }],
        teamName: [{ required: true, message: 'Team Name is required', trigger: 'blur' }]
      },
      downloadLoading: false,
      lstLeader: []
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.listLoading = true
      this.$store.dispatch('GetAllTeam').then(items => {
        if (this.listQuery.teamName) {
          this.list = items.map(item => {
            this.$set(item, 'edit', false)
            return item
          })
            .sort(compareValues(this.listQuery.sort))
            .filter(item => item.teamName.toLowerCase().startsWith(this.listQuery.teamName.toLowerCase()));
        } else {
          this.list = items.map(item => {
            this.$set(item, 'edit', false)
            return item
          }).sort(compareValues(this.listQuery.sort));
        }
      });
      this.$store.dispatch('GetAllUser').then(items => {
        this.lstLeader = items.filter(user => user.role === 'Manager')
      })
      this.listLoading = false;
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
      this.$message({
        message: 'Success',
        type: 'success'
      })
      row.status = status
    },
    resetTemp() {
      this.temp = {
        id: undefined,
        teamName: '',
        leader: '',
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
          this.$store.dispatch('CreateTeam', this.temp).then(res => {
            console.log(res);
          });
        }
      })
    },
    handleUpdate(row) {
      this.temp = Object.assign({}, row) // copy obj
      this.temp.timestamp = new Date(this.temp.timestamp)
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
          this.$store.dispatch('UpdateTeam', tempData).then(res => {
            console.log(res);
          });
        }
      })
    },
    handleDelete(row) {
      this.$notify({
        title: 'Success',
        message: 'Delete successfully',
        type: 'success',
        duration: 2000
      })
      const index = this.list.indexOf(row)
      this.list.splice(index, 1)
    },
    handleDownload() {
      this.downloadLoading = true
      import('@/vendor/Export2Excel').then(excel => {
        const tHeader = ['id', 'teamName', 'createdDate', 'lastModifyDate', 'leader']
        const filterVal = ['id', 'teamName', 'createdDate', 'lastModifyDate', 'leader']
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

