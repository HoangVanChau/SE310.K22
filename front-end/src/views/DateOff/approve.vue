<template>
  <div class="app-container">
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
      <!-- <el-table-column :label="$t('table.teamName')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.fullName }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.createdDate')" align="center" sortable prop="createdDate">
        <template slot-scope="scope" >
          <div>{{ scope.row.fromDate | parseTime('{d}-{m}-{y}') }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.lastModifiedDate')" align="center">
        <template slot-scope="scope">
          <div>{{ scope.row.toDate | parseTime('{d}-{m}-{y}') }}</div>
        </template>
      </el-table-column> -->
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width" >
        <template slot-scope="scope">
          <el-button type="primary" size="medium" icon="el-icon-detail" @click="handleDetail(scope.row)"/>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import ConfirmDialog from '@/components/ConfirmDialog'
import waves from '@/directive/waves'

export default {
  name: 'Approve',
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
      listLoading: true,
    }
  }
}
</script>

<style>

</style>
