<template>
  <div class="tab-container">
    <!-- <el-alert :closable="false" title="Tab with keep-alive" type="success"/> -->
    <el-tabs v-model="activeName" style="margin-top:15px;" type="border-card">
      <el-tab-pane v-for="item in tabMapOptions" :label="item.label" :key="item.key" :name="item.key">
        <keep-alive>
          <profile-tab-pane v-if="activeName==item.key" :type="item.key" @create="showCreatedTimes"/>
        </keep-alive>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import profileTabPane from './profileTabPane.vue';

export default {
  name: 'ProfileTab',
  components: { profileTabPane },
  data() {
    return {
      tabMapOptions: [
        { label: 'Thông tin', key: 'home' },
        { label: 'Mật khẩu', key: 'auth' },
        { label: 'Điểm danh', key: 'attendance' },
      ],
      activeName: 'home',
      createdTimes: 0
    }
  },
  methods: {
    showCreatedTimes() {
      this.createdTimes = this.createdTimes + 1
    }
  }
}
</script>

<style scoped>
  .tab-container{
    margin: 30px;
  }
</style>
