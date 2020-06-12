<template>
  <div class="app-container">
    <upload-excel-component :on-success="handleSuccess" :before-upload="beforeUpload"/>
    <el-dialog :visible.sync="preview" title="Preview" width="90%" @close="preview = false">
      <el-table :data="tableData" border highlight-current-row style="width: 100%;margin-top:20px;">
        <el-table-column v-for="item of tableHeader" :prop="item" :label="item" :key="item"/>
      </el-table>
      <div slot="footer" class="dialog-footer">
        <el-button @click="preview = false">{{ $t('table.cancel') }}</el-button>
        <el-button type="primary" @click="save">{{ $t('table.save') }}</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import UploadExcelComponent from '@/components/UploadExcel/index.vue'

export default {
  name: 'UploadExcel',
  components: { UploadExcelComponent },
  data() {
    return {
      tableData: [],
      tableHeader: [],
      preview: false,
      file: {}
    }
  },
  methods: {
    beforeUpload(file) {
      const isLt1M = file.size / 1024 / 1024 < 1

      if (isLt1M) {
        return true
      }

      this.$notify({
        message: 'PUpload file dưới 1mb.',
        type: 'warning'
      })
      return false
    },
    handleSuccess({ results, header, file }) {
      this.tableData = results
      this.tableHeader = header
      this.file = file
      this.preview = true
    },
    save() {
      console.log('this.file :>> ', this.file);
      this.$store.dispatch('ImportAndInsert', this.file).then(res => {
        if (res) {
          this.preview = false;
          this.$notify({
            title: 'Thành công',
            message: 'Tạo thành công',
            type: 'success',
            duration: 2000
          })
        }
      })
    }
  }
}
</script>
