<template>
  <div class="text-center">
    <img ref="output" :src="avartar" class="avatar img-circle img-thumbnail" alt="avatar">
    <h6>Upload a different photo...</h6>
    <input
      id="file"
      ref="file"
      :disabled="isSaving"
      type="file"
      class="text-center center-block file-upload"
      @change="upload">
  </div>
</template>

<script>
export default {
  name: 'UploadAvatar',
  data() {
    return {
      avartar: this.$store.getters.curUser.avatarImageId === null
        ? 'http://ssl.gstatic.com/accounts/ui/avatar_1x.png'
        : 'http://34.80.19.146:5001/api/images?id='.concat(this.$store.getters.curUser.avatarImageId),
      file: {},
      isSaving: false
    }
  },
  watch: {
    avartar: function(val) {
      this.avartar = val
    }
  },
  methods: {
    upload() {
      this.file = this.$refs.file.files[0]
      console.log('this.file :>> ', this.file);
      const objectURL = URL.createObjectURL(this.file)
      console.log('objectURL :>> ', objectURL);
      this.avartar = objectURL
      this.isSaving = true
      this.$store.dispatch('PreviewImage', this.file).then(res => {
        this.isSaving = false
      })
    },
  }
}
</script>

<style>

</style>
