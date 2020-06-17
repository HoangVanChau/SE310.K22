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
      <el-table-column :label="$t('dateOff.name')" align="center">
        <template slot-scope="scope" >
          <div>{{ scope.row.user[0].fullName }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('dateOff.date')" align="center" sortable prop="createdDate">
        <template slot-scope="scope" >
          <div>{{ scope.row.date | parseTime('{d}-{m}-{y}') }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('dateOff.startOff')" align="center">
        <template slot-scope="scope">
          <div>{{ scope.row.startOff }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('dateOff.endOff')" align="center">
        <template slot-scope="scope">
          <div>{{ scope.row.endOff }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('dateOff.rea')" align="center">
        <template slot-scope="scope">
          <div>{{ scope.row.reason }}</div>
        </template>
      </el-table-column>
      <el-table-column :label="$t('dateOff.approved')" align="center">
        <template slot-scope="scope">
          <el-checkbox readonly>{{ scope.row.isApprove }}</el-checkbox>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width" >
        <template slot-scope="scope">
          <el-button type="primary" size="medium" icon="el-icon-detail" @click="handleApprove(scope.row)"/>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import ConfirmDialog from '@/components/ConfirmDialog'
import waves from '@/directive/waves'
import { mapGetters } from 'vuex'

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
  },
  computed: {
    ...mapGetters([
      'curUser',
      'userPermission',
    ])
  },
  created() {
    this.getListDateOffByCurUser();
  },
  methods: {
    getListDateOffByCurUser() {
      this.listLoading = true;
      const params = {
        status: null,
        userId: this.curUser,
        teamId: null,
        fromDate: null,
        toDate: null
      };
      this.$store.dispatch('GetDateOffsByUser', params).then(res => {
        if (res) {
          this.list = res;
          this.listLoading = false;
        }
      })
    },
    handleApprove(row) {
      const params = {
        id: row.id,
        dataParam: row.isApprove
      };
      this.$store.dispatch('ApproveDateOff', params).then(res => {
        if (res) {
          console.log(res);
        }
      })
    }
  }
}
</script>

<style>

</style>
