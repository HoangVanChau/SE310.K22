<template>
  <el-form ref="dateOffForm" :model="dateOff" :rules="dateOffRules" label-position="top">
    <el-form-item prop="reason">
      <el-input v-model="dateOff.reason" name="reason" placeholder="Lí do" style="width: 50%"/>
    </el-form-item>
    <el-form-item prop="date">
      <el-date-picker
        v-model="dateOff.date"
        :picker-options="datePickerOptions"
        type="date"
        placeholder="Ngày nghỉ"
        style="width: 50%"/>
    </el-form-item>
    <el-form-item prop="startOff">
      <el-time-picker
        v-model="dateOff.startOff"
        :picker-options="timePickerOptions"
        type="time"
        format="hh:mm"
        placeholder="Giờ bắt đầu"
        value-format="hh:mm"
        style="width: 50%"/>
    </el-form-item>
    <el-form-item prop="endOff">
      <el-time-picker
        v-model="dateOff.endOff"
        :picker-options="timePickerOptions"
        type="time"
        format="hh:mm"
        placeholder="Giờ Kết thúc"
        value-format="hh:mm"
        style="width: 50%"/>
    </el-form-item>
    <el-form-item>
      <el-select v-model="dateOff.isUnpaidOff" class="filter-item" placeholder="Please select" style="width: 50%" clearable>
        <el-option :value="true" :disabled="curUser.yearRemainDayOffs<=0" label="Nghỉ phép"/>
        <el-option :value="false" label="Nghỉ không lương"/>
      </el-select>
    </el-form-item>
    <el-form-item>
      <el-input v-model="curUser.yearRemainDayOffs" name="remain" style="width: 50%" disabled/>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitDateOff">{{ $t('table.confirm') }}</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
  name: 'Submit',
  data() {
    return {
      dateOff: {
        reason: '',
        startOff: null,
        endOff: null,
        date: null,
        isUnpaidOff: null
      },
      datePickerOptions: {
        disabledDate(date) {
          return date < new Date();
        }
      },
      timePickerOptions: {
        selectableRange: ['09:00:00 - 12:00:00', '13:30:00 - 18:00:00'],
        format: 'hh:mm'
      },
      dateOffRules: {
        reason: [{ required: true, trigger: 'change', message: 'Lí do không để trống' }],
        endOff: [{ required: true, trigger: 'change', message: 'Giờ bắt đầu không để trống' }],
        startOff: [{ required: true, trigger: 'change', message: 'Giờ kết thúc không để trống' }],
        date: [{ required: true, trigger: 'change', message: 'Ngày nghỉ không để trống' }],
      },
      list: []
    }
  },
  computed: {
    ...mapGetters([
      'curUser',
      'userPermission',
    ])
  },
  created() {
    this.$nextTick(() => {
      this.$refs['dateOffForm'].clearValidate()
    })
  },
  methods: {
    submitDateOff() {
      this.$refs['dateOffForm'].validate((valid) => {
        if (valid) {
          this.$store.dispatch('SubmitDateOff', this.dateOff).then(res => {
            this.dialogFormVisible = false
            this.$notify({
              title: 'Success',
              message: 'Create successfully',
              type: 'success',
              duration: 2000
            })
            this.reset();
          });
        }
      })
    },
    reset() {
      this.dateOff = {
        reason: '',
        startOff: null,
        endOff: null,
        date: null
      }
    }
  }
}
</script>

<style>

</style>
