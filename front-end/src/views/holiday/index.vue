<template>
  <div class="app-container">
    <div class="filter-container">
      <el-button v-show="curUser.role == 'Hr'" class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">{{ $t('table.add') }}</el-button>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-refresh" @click="getList">{{ $t('table.refresh') }}</el-button>
    </div>
    <el-table
      v-loading="listLoading"
      :data="list"
      border
      fit
      stripe
      highlight-current-row
      style="width: 100%;"
    >
      <el-table-column :label="$t('holiday.date')" align="center" prop="date">
        <template slot-scope="scope" >
          <div>{{ scope.row.date }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('holiday.description')" align="center" prop="description">
        <template slot-scope="scope" >
          <div>{{ scope.row.description }}</div>
        </template>
      </el-table-column>
      <el-table-column v-if="curUser.role == 'Hr'" :label="$t('table.actions')" align="center" class-name="small-padding fixed-width" >
        <template slot-scope="scope">
          <!-- <el-button type="primary" size="medium" icon="el-icon-edit" @click="handleUpdate(scope.row)"/> -->
          <el-button
            size="medium"
            type="danger"
            icon="el-icon-delete"
            @click="handleDelete(scope.row)"/>
        </template>
      </el-table-column>
    </el-table>

    <div class="pagination-container">
      <el-pagination v-show="total>0" :current-page="listQuery.page" :page-sizes="[10,20,30, 50]" :page-size="listQuery.limit" :total="total" background layout="total, sizes, prev, pager, next, jumper" @size-change="handleSizeChange" @current-change="handleCurrentChange"/>
    </div>

    <el-dialog :visible.sync="dialogFormVisible" title="Thêm ngày lễ">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="150px" style="width: 70%; margin-left:50px;">
        <el-form-item :label="$t('holiday.date')" prop="date">
          <el-date-picker
            v-model="temp.date"
            type="date"
            format="MM/dd/yyyy"
            style="width: 100%"/>
        </el-form-item>
        <el-form-item :label="$t('holiday.description')" prop="description">
          <el-input v-model="temp.description"/>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button type="primary" @click="createData">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <confirm-dialog :confirm="confirm" :data="temp.description" :call-back="deleteData"/>

  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import waves from '@/directive/waves'
import ConfirmDialog from '@/components/ConfirmDialog'

export default {
  name: 'Holiday',
  directives: {
    waves
  },
  components: { ConfirmDialog },
  data() {
    return {
      list: null,
      total: null,
      listLoading: true,
      listQuery: {
        page: 1,
        limit: 10,
        // importance: undefined,
        // type: undefined,
        sort: '+createdDate'
      },
      temp: {
        description: null,
        id: null,
        date: null
      },
      rules: {
        date: [{ required: true, trigger: 'change', message: 'Ngàykhông để trống' }],
        description: [{ required: true, trigger: 'change', message: 'Tên ngày lễ không để trống' }],
      },
      dialogFormVisible: false,
      confirm: false
    }
  },
  computed: {
    ...mapGetters([
      'curUser',
    ])
  },
  created() {
    this.getList();
  },
  methods: {
    resetTemp() {
      this.temp = {
        description: null,
        id: null,
        date: null
      }
    },
    handleCreate(row) {
      this.resetTemp()
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    handleDelete(row) {
      this.temp = Object.assign({}, row);
      this.confirm = true
    },
    deleteData() {
      this.$store.dispatch('DeleteHoliday', this.temp.id).then(res => {
        if (res) {
          this.$notify({
            title: 'Thành công',
            message: 'Xóa thành công',
            type: 'success',
            duration: 2000
          })
          this.confirm = false;
          this.getList();
        }
      })
    },
    createData() {
      this.$store.dispatch('AddHoliday', this.temp).then(res => {
        if (res) {
          this.$notify({
            title: 'Thành công',
            message: 'Tạo thành công',
            type: 'success',
            duration: 2000
          })
          this.dialogFormVisible = false;
        }
        this.getList();
      })
    },
    getList() {
      this.$store.dispatch('GetAllHoliday').then(res => {
        if (res) {
          this.list = res;
          this.listLoading = false;
        }
      })
    },
    handleSizeChange(val) {
      this.listQuery.limit = val
      this.getList()
    },
    handleCurrentChange(val) {
      this.listQuery.page = val
      this.getList()
    },
  }
}
</script>

<style>

</style>
