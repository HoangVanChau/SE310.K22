<template>
  <el-dialog :visible.sync="confirm" title="Xác nhận" @close="{onClose}">
    <div class="" style="width: 70%; margin-left: 50px;">
      {{ $t('confirm.deleteMes') }}
      {{ data }}
    </div>
    <div slot="footer" class="dialog-footer">
      <el-button @click="confirm = false">{{ $t('table.cancel') }}</el-button>
      <el-button type="primary" @click="callBack">{{ $t('table.confirm') }}</el-button>
    </div>
  </el-dialog>
</template>

<script>
export default {
  name: 'ConfirmDialog',
  props: {
    confirm: {
      type: Boolean,
      default: false
    },
    data: {
      type: String,
      default: ''
    },
    callBack: {
      type: Function,
      default: () => {}
    },
    onClose: {
      type: Function,
      default: () => {}
    }
  }
}
</script>

<style>

</style>
