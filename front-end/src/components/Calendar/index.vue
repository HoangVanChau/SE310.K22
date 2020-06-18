<template>
  <!-- :events="[
      { title: 'event 1', date: '2020-05-24' }
    ]" -->
  <div class="">
    <FullCalendar
      :plugins="calendarPlugins"
      :events ="data"
      default-view="dayGridMonth"/>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import FullCalendar from '@fullcalendar/vue'
import dayGridPlugin from '@fullcalendar/daygrid'
export default {
  name: 'Calendar',
  components: { FullCalendar },
  data() {
    return {
      day: null,
      date: null,
      month: null,
      data: [],
      numberDate: 0,
      calendarPlugins: [dayGridPlugin]
    }
  },
  computed: {
    ...mapGetters([
      'curUser',
      'userPermission',
    ])
  },
  created() {
    this.getDataByEmpId();
  },
  methods: {
    getMonth() {
      this.month = new Date().getMonth() + 1;
    },
    parseDate(date) {
      var dd = date.getDate();

      var mm = date.getMonth() + 1;
      var yyyy = date.getFullYear();
      if (dd < 10) {
        dd = '0' + dd;
      }

      if (mm < 10) {
        mm = '0' + mm;
      }
      date = mm + '/' + dd + '/' + yyyy;
      return date;
    },
    changeFormat(date) {
      var dd = date.getDate();

      var mm = date.getMonth() + 1;
      var yyyy = date.getFullYear();
      if (dd < 10) {
        dd = '0' + dd;
      }

      if (mm < 10) {
        mm = '0' + mm;
      }
      date = yyyy + '-' + mm + '-' + dd;
      return date;
    },
    getDataByEmpId() {
      const employeeId = this.curUser.employeeId;
      const today = new Date();
      const fromDate = new Date(today.getFullYear(), today.getMonth(), 1);
      const toDate = new Date(today.getFullYear(), today.getMonth() + 1, 0);
      this.numberDate = toDate.getDate();
      const params = {
        fromDate: this.parseDate(fromDate),
        toDate: this.parseDate(toDate),
        employeeId
      }
      this.$store.dispatch('GetDataByIdAndDate', params).then(res => {
        res.forEach(element => {
          const date = new Date(element.unique.date)
          const value = this.changeFormat(date)
          this.data.push({ title: 'Có mặt', date: value });
        });
        this.getHoliday();
        console.log('this.data', this.data);
      })
    },
    getHoliday() {
      this.$store.dispatch('GetAllHoliday').then(res => {
        res.forEach(item => {
          const date = new Date(item.date)
          const value = this.changeFormat(date)
          this.data.push({ title: item.description, date: value })
        })
      })
    }
  }
}
</script>

<style lang="scss">
@import '../../../node_modules/@fullcalendar/core/main.css';
@import '../../../node_modules/@fullcalendar/daygrid/main.css';
</style>
