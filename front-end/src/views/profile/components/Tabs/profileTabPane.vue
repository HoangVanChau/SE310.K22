<template>
  <div id="home" class="tab-pane active">
    <hr>
    <form id="registrationForm" class="form" action="##" method="post">
      <div class="form-group">

        <div class="col-xs-6">
          <label for="first_name"><h4>{{ $t('profile.firstName') }}</h4></label>
          <input id="first_name" v-model="name" type="text" class="form-control" name="first_name" placeholder="first name" title="enter your first name if any.">
        </div>
      </div>
      <div class="form-group">

        <div class="col-xs-6">
          <label for="last_name"><h4>{{ $t('profile.lastName') }}</h4></label>
          <input id="last_name" v-model="name" type="text" class="form-control" name="last_name" placeholder="last name" title="enter your last name if any.">
        </div>
      </div>

      <div class="form-group">

        <div class="col-xs-6">
          <label for="phone"><h4>{{ $t('profile.phone') }}</h4></label>
          <input id="phone" v-model="phone" type="text" class="form-control" name="phone" placeholder="enter phone" title="enter your phone number if any.">
        </div>
      </div>

      <div class="form-group">
        <div class="col-xs-6">
          <label for="dateOfBirth"><h4>{{ $t('profile.dateOfBirth') }}</h4></label>
          <el-date-picker id="dateOfBirth" v-model="dateOfBirth" :placeholder="$t('profile.dateOfBirth')" style="display: block" type="date"/>
        </div>
      </div>
      <div class="form-group">
        <div class="col-xs-6">
          <label for="email"><h4>{{ $t('profile.email') }}</h4></label>
          <input id="email" v-model="email" type="email" class="form-control" name="email" placeholder="you@email.com" title="enter your email.">
        </div>
      </div>
      <div class="form-group">

        <div class="col-xs-6">
          <label for="gender"><h4>{{ $t('profile.gender') }}</h4></label>
          <!-- <input id="location" v-model="gender" type="checkbox" class="form-control" placeholder="somewhere"> -->
          <el-radio-group id="gender" v-model="gender" style="display: block">
            <el-radio label="Nam" border>{{ $t('profile.male') }}</el-radio>
            <el-radio label="Nu" border>{{ $t('profile.female') }}</el-radio>
          </el-radio-group>
        </div>
      </div>
      <div class="form-group">

        <div class="col-xs-6">
          <label for="password"><h4>{{ $t('profile.password') }}</h4></label>
          <input id="password" type="password" class="form-control" name="password" placeholder="password" title="enter your password.">
        </div>
      </div>
      <div class="form-group">

        <div class="col-xs-6">
          <label for="password2"><h4>{{ $t('profile.verify') }}</h4></label>
          <input id="password2" type="password" class="form-control" name="password2" placeholder="password2" title="enter your password2.">
        </div>
      </div>
      <div class="form-group">
        <div class="col-xs-6">
          <label for="marry"><h4>{{ $t('profile.marryStatus') }}</h4></label>
          <el-checkbox :checked="marryStatus" v-model="marryStatus" style="display: block">Đã kết hôn</el-checkbox>
        </div>
      </div>
      <div class="form-group">
        <div class="col-xs-12">
          <br>
          <button class="btn btn-lg btn-success" type="button" @click="save()"><i class="glyphicon glyphicon-ok-sign"/> {{ $t('profile.save') }}</button>
          <button class="btn btn-lg" type="reset"><i class="glyphicon glyphicon-repeat"/> {{ $t('profile.reset') }}</button>
        </div>
      </div>
    </form>

    <hr>

  </div><!--/tab-pane-->
</template>

<script>
// import { fetchList } from '@/api/article'
import store from '@/store'
import NProgress from 'nprogress'; // progress bar
import 'nprogress/nprogress.css'; // progress bar style

NProgress.configure({ showSpinner: false }); // NProgress Configuration

export default {
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'info',
        deleted: 'danger'
      }
      return statusMap[status]
    }
  },
  props: {
    type: {
      type: String,
      default: 'home'
    },
    account: {
      type: Object,
      default: () => store.getters.account
    },
  },
  data() {
    return {
      list: null,
      listQuery: {
        page: 1,
        limit: 5,
        type: this.type,
        sort: '+id'
      },
      loading: false,
      updatedData: { ...this.account }
    }
  },
  computed: {
    name: {
      get: function() {
        return this.updatedData.name
      },
      set: function(v) {
        this.updatedData.name = v
      }
    },
    email: {
      get: function() {
        return this.updatedData.email
      },
      set: function(v) {
        this.updatedData.email = v
      }
    },
    phone: {
      get: function() {
        return this.updatedData.phone
      },
      set: function(v) {
        this.updatedData.phone = v
      }
    },
    gender: {
      get: function() {
        return this.updatedData.gender
      },
      set: function(v) {
        this.updatedData.gender = v
      }
    },
    marryStatus: {
      get: function() {
        return this.updatedData.marryStatus
      },
      set: function(v) {
        this.updatedData.marryStatus = v
      }
    },
    dateOfBirth: {
      get: function() {
        return this.updatedData.dateOfBirth
      },
      set: function(v) {
        this.updatedData.dateOfBirth = v
      }
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      // this.loading = true
      // this.$emit('create') // for test
      // fetchList(this.listQuery).then(response => {
      //   this.list = response.data.items
      //   this.loading = false
      // })
    },
    save() {
      // this.loading = true
      NProgress.start();
      this.$emit('update')
      console.log('this.$store.getters.newAvatar', this.$store.getters.newAvatar);
      // bug update the same avatar
      this.$store.dispatch('UpdateUser', {
        id: this.$store.getters.userId,
        data: this.updatedData,
        file: this.$store.getters.newAvatar === this.updatedData.avatar ? null : this.$store.getters.newAvatar }).then(res => {
        NProgress.done()
      })
    }
  }
}
</script>

