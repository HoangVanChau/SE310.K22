<template>
  <div class="container">
    <div class="row">
      <div class="col-sm-10"><h1>User name</h1></div>
    </div>
    <div class="row">
      <div class="col-sm-3"><!--left col-->

        <!-- <div class="text-center">
          <img src="http://ssl.gstatic.com/accounts/ui/avatar_2x.png" class="avatar img-circle img-thumbnail" alt="avatar">
          <h6>Upload a different photo...</h6>
          <input type="file" class="text-center center-block file-upload">
        </div> -->
        <upload-avatar />
        <hr>
        <br>

        <div class="panel panel-default">
          <div class="panel-heading">Website <i class="fa fa-link fa-1x"/></div>
          <div class="panel-body"><a href="http://bootnipets.com">bootnipets.com</a></div>
        </div>

        <ul class="list-group">
          <li class="list-group-item text-muted">Activity <i class="fa fa-dashboard fa-1x"/></li>
          <li class="list-group-item text-right"><span class="pull-left"><strong>Shares</strong></span> 125</li>
          <li class="list-group-item text-right"><span class="pull-left"><strong>Likes</strong></span> 13</li>
          <li class="list-group-item text-right"><span class="pull-left"><strong>Posts</strong></span> 37</li>
          <li class="list-group-item text-right"><span class="pull-left"><strong>Followers</strong></span> 78</li>
        </ul>

      </div><!--/col-3-->
      <div class="col-sm-9">
        <profile-tab/>
      </div>

    </div><!--/col-9-->
  </div><!--/row-->
</template>
<script>
import singleImage from '@/components/Upload/singleImage.vue';
import profileTab from './components/Tabs/profileTab.vue';
import uploadAvatar from './components/UploadAvatar/uploadAvatar.vue';

export default {
  name: 'Profile',
  components: { singleImage, profileTab, uploadAvatar },
}
</script>
