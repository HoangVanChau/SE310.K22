<template>
  <div class="app-container">
    <date-off-tab/>
  </div>
</template>

<script>
import MdInput from '@/components/MDinput'
import dateOffTab from './components/dateOffTab'
// import { mapGetters } from 'vuex';
export default {
  name: 'DateOff',
  components: {
    MdInput,
    dateOffTab
  },

  data() {
    return {}
  },
  computed: {

  },
  methods: {
  }
}
</script>

<style>

</style>
