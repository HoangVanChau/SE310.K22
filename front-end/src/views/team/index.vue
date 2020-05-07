<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input :placeholder="$t('table.teamName')" v-model="listQuery.teamName" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.sort" style="width: 160px" class="filter-item" @change="handleFilter">
        <el-option v-for="item in sortOptions" :key="item.key" :label="item.label" :value="item.key"/>
      </el-select>
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
      <!-- <el-table-column :label="$t('table.id')" align="center" hidden="true">
      <template slot-scope="scope">
          <div>{{ scope.row.teamId }}</div>
        </template>
      </el-table-column> -->
      <el-table-column :label="$t('table.leader')" align="center">
        <template slot-scope="scope" >
          <div @contextmenu.prevent="$refs['menu'].open($event, scope.row)" >{{ scope.row.leaders[0].fullName }}</div>
          <!-- <div>{{ scope.row.leaders[0].fullName }}</div> -->
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.createdDate')" align="center" sortable prop="createdDate">
        <template slot-scope="scope" >
          <div @contextmenu.prevent="$refs['menu'].open($event, scope.row)">{{ scope.row.createdDate | parseTime('{d}-{m}-{y}') }}</div>
          <!-- <div>{{ scope.row.createdDate | parseTime('{d}-{m}-{y}') }}</div> -->
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.lastModifiedDate')" align="center">
        <template slot-scope="scope">
          <div @contextmenu.prevent="$refs['menu'].open($event, scope.row)">{{ scope.row.lastModifyDate | parseTime('{d}-{m}-{y}') }}</div>
          <!-- <div>{{ scope.row.lastModifyDate | parseTime('{d}-{m}-{y}') }}</div> -->
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.teamName')" align="center">
        <template slot-scope="scope" >
          <div @contextmenu.prevent="$refs['menu'].open($event, scope.row)">{{ scope.row.teamName }}</div>
          <!-- <div>{{ scope.row.teamName }}</div> -->
        </template>
      </el-table-column>
      <el-table-column v-if="userPermission" :label="$t('table.actions')" align="center" width="250" class-name="small-padding fixed-width" >
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

    <vue-context ref="menu" >
      <template v-if="child.data" slot-scope="child">
        <li>
          <a @click.prevent="handleAddUser(child.data)">
            Add user to team <strong>{{ child.data.teamName }}</strong>
          </a>
        </li>
        <li>
          <a @click.prevent="handleRemoveUser(child.data)">
            Remove user to team <strong>{{ child.data.teamName }}</strong>
          </a>
        </li>
      </template>
    </vue-context>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogExcUser" @close="onClose">
      <div class="row">
        <div class="col-xs-5 col-md-5 text-center" >
          <h4>CURRENT USER</h4>
          <div
            class="mt-15"
            data-spy="scroll"
            data-offset="0"
            style="height: 310px; overflow-y: auto">
            <ul class="list-group">
              <li
                v-for="user in employees"
                :key="user.userId"
                :class="{'active': selectedMemberId.includes(user.userId), 'disabled': dialogStatus == 'removeUser'}"
                class="list-group-item"
                @click="onSelectedMemberId(user,'add')">
                {{ user.fullName }}
              </li>
            </ul>
          </div>
        </div>
        <div class="col-xs-2 col-md-2 text-center">
          <h4>ACTION</h4>
          <div class="mt-15">
            <div class="row" style="margin-top: 5px">
              <el-button id="a-one" :disabled="dialogStatus=='removeUser'" plain icon="el-icon-arrow-right icon-fs" @click.prevent="onClick('addOne')"/>
            </div>
            <div class="row" style="margin-top: 5px">
              <el-button id="a-all" :disabled="dialogStatus=='removeUser'" plain icon="el-icon-d-arrow-right icon-fs" @click.prevent="onClick('addAll')"/>
            </div>
            <div class="row" style="margin-top: 5px">
              <el-button id="r-one" :disabled="dialogStatus=='addUser'" plain icon="el-icon-arrow-left icon-fs" @click.prevent="onClick('removeOne')"/>
            </div>
            <div class="row" style="margin-top: 5px">
              <el-button id="r-all" :disabled="dialogStatus=='addUser'" plain icon="el-icon-d-arrow-left icon-fs" @click.prevent="onClick('removeAll')"/>
            </div>
          </div>
        </div>
        <div class="col-xs-5 col-md-5 text-center">
          <h4>USER IN TEAM</h4>
          <div class="mt-15" style="height: 300px; overflow-y: auto">
            <ul class="list-group">
              <li
                v-for="member in selectedTeam.members"
                :key="member.userId"
                :class="{'active': selectedMemberIdDeleted.includes(member.userId), 'disabled': dialogStatus == 'addUser'}"
                class="list-group-item"
                @click="onSelectedMemberId(member,'remove')">
                {{ member.fullName }}
              </li>
            </ul>
          </div>
        </div>
      </div>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogExcUser = false">{{ $t('table.cancel') }}</el-button>
      </div>
    </el-dialog>

    <confirm-dialog :confirm="confirm" :data="temp.teamId" :call-back="handleDelete" :on-close="()=>confirm = false"/>

  </div>
</template>

<script>
import waves from '@/directive/waves' // 水波纹指令
import { parseTime, compareValues } from '@/utils'
import { mapGetters } from 'vuex'
import VueContext from 'vue-context'
import ConfirmDialog from '@/components/ConfirmDialog'
import TeamContextMenu from './component/TeamContextMenu/TeamContextMenu'
import 'vue-context/src/sass/vue-context.scss';

export default {
  name: 'LstTeam',
  directives: {
    waves
  },
  components: { VueContext, ConfirmDialog, TeamContextMenu },
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
        teamName: undefined,
        // type: undefined,
        sort: '+createdDate'
      },
      sortOptions: [{ label: 'Date Ascending', key: '+createdDate' }, { label: 'Date Descending', key: '-createdDate' }],
      temp: {
        id: undefined,
        teamName: '',
        leaderId: '',
        teamId: ''
      },
      dialogFormVisible: false,
      dialogExcUser: false,
      dialogStatus: '',
      textMap: {
        update: 'Edit',
        create: 'Create',
        addUser: 'Add user to Team',
        removeUser: 'Remove user to Team',
      },
      rules: {
        leaderId: [{ required: true, message: 'leader is required', trigger: 'change' }],
        teamName: [{ required: true, message: 'Team Name is required', trigger: 'blur' }]
      },
      downloadLoading: false,
      lstLeader: [],
      selectedTeam: {},
      selectedMemberId: [],
      selectedMemberIdDeleted: [],
      userNotInTeam: [],
      confirm: false,
      menuVisible: false,
      employees: []
    }
  },
  computed: {
    ...mapGetters([
      'curUser',
      'userPermission',
      'users'
    ])
  },
  created() {
    this.getList()
  },
  methods: {
    onClose() {
      this.selectedMemberId = []
      this.selectedMemberIdDeleted = []
    },
    onClick(type) {
      switch (type) {
        // eslint-disable-next-line no-case-declarations
        case 'addOne':
          const addOne = {
            teamId: this.selectedTeam.teamId,
            userId: this.selectedMemberId[0]
          }
          this.$store.dispatch('AddUserToTeam', addOne).then(res => {
            if (res) {
              console.log('addOne :>> ', res);
              this.$notify({
                title: 'Success',
                message: 'Update successfully',
                type: 'success',
                duration: 2000
              })
              this.fetchDataExcUser(this.selectedTeam)
            }
          })
          break;
        // eslint-disable-next-line no-case-declarations
        case 'addAll':
          const addAll = {
            teamId: this.selectedTeam.teamId,
            members: this.selectedMemberId
          }
          this.$store.dispatch('AddUsersToTeam', addAll).then(res => {
            if (res) {
              console.log('addOne :>> ', res);
              this.$notify({
                title: 'Success',
                message: 'Update successfully',
                type: 'success',
                duration: 2000
              })
              this.fetchDataExcUser(this.selectedTeam)
            }
          })
          break;
        // eslint-disable-next-line no-case-declarations
        case 'removeOne':
          const removeOne = {
            teamId: this.selectedTeam.teamId,
            userId: this.selectedMemberIdDeleted[0]
          }
          this.$store.dispatch('RemoveUserToTeam', removeOne).then(res => {
            if (res) {
              console.log('removeOne :>> ', res);
              this.$notify({
                title: 'Success',
                message: 'Remove successfully',
                type: 'success',
                duration: 2000
              })
              this.fetchDataExcUser(this.selectedTeam)
            }
          })
          break;
        // eslint-disable-next-line no-case-declarations
        case 'removeAll':
          const removeAll = {
            teamId: this.selectedTeam.teamId,
            members: this.selectedMemberIdDeleted
          }
          this.$store.dispatch('RemoveUserToTeam', removeAll).then(res => {
            if (res) {
              console.log('removeAll :>> ', res);
              this.$notify({
                title: 'Success',
                message: 'Remove successfully',
                type: 'success',
                duration: 2000
              })
              this.fetchDataExcUser(this.selectedTeam)
            }
          })
          break;

        default:
          break;
      }
    },
    onSelectedMemberId(user, type) {
      if (type === 'add') {
        if (!this.selectedMemberId.includes(user.userId)) {
          this.selectedMemberId.push(user.userId)
        } else {
          const index = this.selectedMemberId.indexOf(user.userId)
          this.selectedMemberId.splice(index, 1)
        }
        console.log('this.selectedMemberId', this.selectedMemberId);
      } else {
        if (!this.selectedMemberIdDeleted.includes(user.userId)) {
          this.selectedMemberIdDeleted.push(user.userId)
        } else {
          const index = this.selectedMemberIdDeleted.indexOf(user.userId)
          this.selectedMemberIdDeleted.splice(index, 1)
        }
        console.log('this.selectedMemberIdDeleted', this.selectedMemberIdDeleted);
      }
    },
    fetchDataExcUser(team) {
      const teamId = team.teamId
      const users = this.users
      this.$store.dispatch('GetTeamByID', teamId).then(res => {
        if (res) {
          this.selectedTeam = res;
          this.$store.dispatch('GetUserNotInTeam', { users, teamId }).then(res => {
            if (res) {
              this.userNotInTeam = res.filter(item => item.role === 'Employee')
            }
          })
        }
      }).catch(e => {

      })
    },
    handleAddUser(row) {
      this.dialogExcUser = true
      this.dialogStatus = 'addUser'
      this.fetchDataExcUser(row)
    },
    addUserToTeam() {
      alert('addUserToTeam')
    },
    handleRemoveUser(row) {
      this.dialogExcUser = true
      this.dialogStatus = 'removeUser'
      this.fetchDataExcUser(row)
    },
    removeUserToTeam() {
      alert('removeUserToTeam')
    },
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
        this.total = items.length;
      });
      this.$store.dispatch('GetLeaderFree').then(items => {
        this.lstLeader = items;
      })
      this.$store.dispatch('GetEmployeeFree').then(items => {
        this.employees = items
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
      this.confirm = true;
      this.temp = Object.assign({}, row)
    },
    handleDelete() {
      this.$store.dispatch('DeleteTeam', this.temp.teamId).then(res => {
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
            this.dialogFormVisible = false
            this.$notify({
              title: 'Success',
              message: 'Create successfully',
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
          const tempData = {
            teamName: this.temp.teamName,
            leaderId: this.temp.leaderId,
            teamAvatarImageId: null
          }
          console.log(this.temp);

          this.$store.dispatch('UpdateTeam', { teamId: this.temp.teamId, newTeam: tempData }).then(res => {
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
