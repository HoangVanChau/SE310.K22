<template>
  <div id="attendance">
    <hr>
    <calendar/>

    <hr>
  </div>
</template>

<script>
import treeTable from '@/components/TreeTable'
import calendar from '@/components/Calendar'
export default {
  name: 'AttendanceTabPane',
  components: { treeTable, calendar },
  props: {},
  data() {
    return {

    }
  },
  created() {

  },
  methods: {

  }
}
</script>

<style>

</style>
