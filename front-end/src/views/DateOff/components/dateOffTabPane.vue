<template>
  <div class="pt-0">
    <div v-if="type=='my'" id="my" class="tab-pane active">
      <hr>
      <my-date-off/>
      <hr>
    </div><!--/tab-pane-->
    <div v-if="type=='submit'" id="submit">
      <hr>
      <submit/>
      <hr>
    </div>
  </div>
</template>

<script>
import submit from './submit'
import myDateOff from './myDateOff'
export default {
  components: { submit, myDateOff },
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'info',
        deleted: 'danger',
      }
      return statusMap[status]
    }
  },
  props: {
    type: {
      type: String,
      default: 'home'
    }
  },
}
</script>

<style>

</style>
