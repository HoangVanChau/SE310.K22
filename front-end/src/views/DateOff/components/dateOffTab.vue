<template>
  <div class="tab-container">
    <!-- <el-alert :closable="false" title="Tab with keep-alive" type="success"/> -->
    <el-tabs v-model="activeName" style="margin-top:15px;" type="border-card">
      <el-tab-pane v-for="item in tabMapOptions" :label="item.label" :key="item.key" :name="item.key">
        <keep-alive>
          <date-off-tab-pane v-if="activeName==item.key" :type="item.key" @create="showCreatedTimes"/>
        </keep-alive>
      </el-tab-pane>
    </el-tabs>
  </div>
</template>

<script>
import dateOffTabPane from './dateOffTabPane.vue';

export default {
  name: 'DateOffTab',
  components: { dateOffTabPane },
  data() {
    return {
      tabMapOptions: [
        { label: 'Danh sách xin nghỉ', key: 'my' },
        { label: 'Xin nghỉ', key: 'submit' },
      ],
      activeName: 'my',
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
