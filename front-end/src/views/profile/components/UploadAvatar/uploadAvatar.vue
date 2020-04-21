<template>
  <div class="text-center">
    <img :src="account.avatar" class="avatar img-circle img-thumbnail" style="width: 150px; height: 150px" alt="avatar">
    <h6>Upload a different photo...</h6>
    <input type="file" class="text-center center-block file-upload" @change="uploadAvatar">
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'UploadAvatar',
  data: function() {
    return {
      cloudinary: {
        uploadPreset: 'hrm',
        apiKey: '751752335667179',
        cloudName: 'critplease'
      }
    }
  },
  computed: {
    ...mapGetters([
      'account'
    ])
  },
  methods: {
    uploadAvatar(e) {
      const files = e.target.files || e.dataTransfer.files;
      if (!files.length) { return; }

      this.$store.dispatch('UpdateAvatar', files[0]).then(res => {
        console.log('this.$store.getters.newAvatar', this.$store.getters.newAvatar);
      })
    }
  }
}
</script>
