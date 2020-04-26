<template>
  <div class="">
    <div v-if="type=='home'" id="home" class="tab-pane active">
      <hr>
      <form id="infoForm" class="form" action="##">
        <div class="form-group">

          <div class="col-xs-6">
            <label for="full_name"><h4>Full name</h4></label>
            <input id="full_name" v-model="curUser.fullName" type="text" class="form-control" name="full_name" placeholder="full name" title="enter your full name if any.">
          </div>
        </div>
        <div class="form-group">

          <div class="col-xs-6">
            <label for="date_of_birth"><h4>Date of birth</h4></label>
            <el-date-picker id="date_of_birth" v-model="curUser.dateOfBirth" style="width: 100%" type="date" format="dd-MM-yyyy" placeholder="Date of birth"/>
          </div>
        </div>

        <div class="form-group">

          <div class="col-xs-6">
            <label for="phone"><h4>Phone</h4></label>
            <input id="phone" v-model="curUser.phoneNumber" type="text" class="form-control" name="phone" placeholder="enter phone" title="enter your phone number if any.">
          </div>
        </div>

        <div class="form-group">
          <div class="col-xs-6">
            <label for="username"><h4>Username</h4></label>
            <input id="username" v-model="curUser.userName" type="text" class="form-control" name="username" placeholder="enter username" title="enter your username if any." readonly>
          </div>
        </div>
        <div class="form-group">

          <div class="col-xs-6">
            <label for="email"><h4>Email</h4></label>
            <input id="email" v-model="curUser.email" type="email" class="form-control" name="email" placeholder="you@email.com" title="enter your email.">
          </div>
        </div>
        <div class="form-group">

          <div class="col-xs-6">
            <label for="address"><h4>Address</h4></label>
            <input id="address" v-model="curUser.address" type="text" class="form-control" placeholder="somewhere" title="enter a address">
          </div>
        </div>
        <div class="form-group">
          <div class="col-xs-12">
            <br>
            <button class="btn btn-lg btn-success" @click="saveHomeTab()">
              <i class="glyphicon glyphicon-ok-sign">
                Save
              </i>
            </button>
            <button class="btn btn-lg" type="reset">
              <i class="glyphicon glyphicon-repeat">
                Reset
              </i>
            </button>
          </div>
        </div>
      </form>

      <hr>

    </div><!--/tab-pane-->
    <div v-if="type=='auth'" id="auth">
      <hr>
      <form id="verifyPassForm" class="form" action="##" method="post">
        <div class="form-group">

          <div class="col-xs-6">
            <label for="password"><h4>Password</h4></label>
            <input id="password" type="password" class="form-control" name="password" placeholder="password" title="enter your password.">
          </div>
        </div>
        <div class="form-group">

          <div class="col-xs-6">
            <label for="password2"><h4>Verify</h4></label>
            <input id="password2" type="password" class="form-control" name="password2" placeholder="password2" title="enter your password2.">
          </div>
        </div>
        <div class="form-group">
          <div class="col-xs-12">
            <br>
            <button class="btn btn-lg btn-success" type="submit"><i class="glyphicon glyphicon-ok-sign"/> Save</button>
            <button class="btn btn-lg" type="reset"><i class="glyphicon glyphicon-repeat"/> Reset</button>
          </div>
        </div>
      </form>

      <hr>
    </div>
  </div>
</template>

<script>
// import { fetchList } from '@/api/article'

export default {
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
      curUser: this.$store.getters.curUser
    }
  },
  computed: {
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.loading = true
      this.$emit('create') // for test
      // fetchList(this.listQuery).then(response => {
      //   this.list = response.data.items
      //   this.loading = false
      // })
    },
    saveHomeTab() {
      console.log(this.curUser);
      this.$store.dispatch('UpdateCurUser', this.curUser).then(user => console.log(user));
    }
  }
}
</script>

