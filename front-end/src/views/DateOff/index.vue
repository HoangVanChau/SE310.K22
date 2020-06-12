<template>
  <div class="app-container">
    <el-form :model="dateOff" :rules="dateOffRules" label-position="top">
      <el-form-item prop="reason">
        <el-input v-model="dateOff.reason" name="reason" placeholder="Lí do"/>
      </el-form-item>
      <el-form-item prop="date">
        <el-date-picker
          v-model="dateOff.fromDate"
          type="daterange"
          range-separator="To"
          start-placeholder="Ngày bắt đầu"
          end-placeholder="Ngày Kết thúc"/>
      </el-form-item>
      <el-form-item prop="startOff">
        <el-time-picker v-model="dateOff.startOff" type="time" format="hh:mm" placeholder="Giờ bắt đầu"/>
      </el-form-item>
      <el-form-item prop="endOff">
        <el-time-picker v-model="dateOff.endOff" type="time" format="hh:mm" placeholder="Giờ Kết thúc"/>
      </el-form-item>

    </el-form>
  </div>
</template>

<script>
import MdInput from '@/components/MDinput'
export default {
  name: 'DateOff',
  components: {
    MdInput,
  },
  data() {
    return {
      dateOff: {
        reason: '',
        startOff: new Date(),
        endOff: new Date(),
      },
      dateOffRules: {
        reason: [{ required: true, trigger: 'change', message: 'Lí do không để trống' }],
        endOff: [{ required: true, trigger: 'change', message: 'Ngày bắt đầu không để trống' }],
        startOff: [{ required: true, trigger: 'change', message: 'Ngày kết thúc không để trống' }],
      }
    }
  }
}
</script>

<style>

</style>
