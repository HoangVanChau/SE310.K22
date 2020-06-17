<template>
  <el-dialog :visible.sync="visible" title="Create Position" width="70%" @close="handleCancel">
    <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="150px">
      <el-form-item :label="$t('table.leader')" prop="leaderId">
        <template v-if="lstLeader.length > 0">
          <el-select v-model="temp.leaderId" class="filter-item" placeholder="Please select" style="width: 100%">
            <el-option v-for="item in lstLeader" :key="item.userId" :label="item.fullName" :value="item.userId"/>
          </el-select>
        </template>
        <template v-else>
          Không còn leader trống
        </template>
      </el-form-item>
      <el-form-item :label="$t('table.teamName')" prop="teamName">
        <el-input v-model="temp.teamName"/>
      </el-form-item>
    </el-form>
    <div slot="footer" class="dialog-footer">
      <el-button @click="handleCancel">{{ $t('table.cancel') }}</el-button>
      <el-button type="primary" @click="createData">{{ $t('table.confirm') }}</el-button>
    </div>
  </el-dialog>
</template>

<script>
export default {
  name: 'AddTeam',
  data() {
    return {
      temp: {
        teamName: '',
        leaderId: '',
        teamId: ''
      },
      visible: false,
      rules: {
        leaderId: [{ required: true, message: 'leader is required', trigger: 'change' }],
        teamName: [{ required: true, message: 'Team Name is required', trigger: 'blur' }]
      },
      lstLeader: []
    }
  },
  created() {
    this.getLeaders();
  },
  methods: {
    clearValidate() {
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    getLeaders() {
      this.$store.dispatch('GetLeaderFree').then(items => {
        this.lstLeader = items;
      })
    },
    resetTemp() {
      this.temp = {
        teamName: '',
        leaderId: '',
        teamId: ''
      }
    },
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.$store.dispatch('CreatePosition', this.temp).then(res => {
            this.$notify({
              title: 'Thành công',
              message: 'Tạo thành công',
              type: 'success',
              duration: 2000
            })
            this.$emit('created', true);
            this.handleCancel();
          });
        }
      })
    },
    handleCancel() {
      this.visible = false;
      this.$emit('close', this.visible);
    },
    handleOpen() {
      this.visible = true;
      this.resetTemp()
      this.clearValidate()
    }
  }
}
</script>

<style>

</style>
