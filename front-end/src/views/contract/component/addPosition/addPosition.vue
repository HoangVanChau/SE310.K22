<template>
  <el-dialog :visible.sync="visible" title="Create Position" width="70%" @close="handleCancel">
    <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="150px">
      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item :label="$t('position.positionName')" prop="positionName">
            <el-input v-model="temp.positionName"/>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label="$t('position.description')" prop="description">
            <el-input v-model="temp.description"/>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item :label="$t('position.baseMonthSalary')" prop="baseMonthSalary">
            <el-input-number v-model="temp.baseMonthSalary" :min="0" :step="100" style="width: 100%"/>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label="$t('position.baseHourSalary')" prop="baseHourSalary">
            <el-input-number v-model="temp.baseHourSalary" :min="0" :step="100" style="width: 100%"/>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item :label="$t('position.baseOtSalaryPerHour')" prop="baseOtSalaryPerHour">
            <el-input-number v-model="temp.baseOtSalaryPerHour" :min="0" :step="100" style="width: 100%"/>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label="$t('position.baseDateOff')" prop="baseDateOff">
            <el-input-number v-model="temp.baseDateOff" :min="0" :step="100" style="width: 100%"/>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row :gutter="20">
        <el-col :span="12">
          <el-form-item :label="$t('position.baseLateMoney')" prop="baseLateMoney">
            <el-input-number v-model="temp.baseLateMoney" :min="0" :step="100" style="width: 100%"/>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <div slot="footer" class="dialog-footer">
      <el-button @click="handleCancel">{{ $t('table.cancel') }}</el-button>
      <el-button type="primary" @click="createData">{{ $t('table.confirm') }}</el-button>
    </div>
  </el-dialog>
</template>

<script>
export default {
  name: 'AddPosition',
  data() {
    return {
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
      visible: false,
      rules: {
        positionName: [{ required: true, message: 'Position Name is required', trigger: 'blur' }],
        baseMonthSalary: [{ required: true, message: 'Base Month Salary is required', trigger: 'blur' }],
        baseHourSalary: [{ required: true, message: 'Base Hour Salary is required', trigger: 'blur' }],
        baseOtSalaryPerHour: [{ required: true, message: 'Base OT Salary per hour is required', trigger: 'blur' }],
        baseDateOff: [{ required: true, message: 'Base Date off is required', trigger: 'blur' }],
        baseLateMoney: [{ required: true, message: 'Base Late Money is required', trigger: 'blur' }],
      },
    }
  },
  created() {
  },
  methods: {
    clearValidate() {
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
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
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.$store.dispatch('CreatePosition', this.temp).then(res => {
            this.$notify({
              title: 'Success',
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
