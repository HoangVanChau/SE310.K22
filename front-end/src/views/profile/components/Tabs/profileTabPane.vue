<template>
  <div id="home" class="tab-pane active">
    <hr>
    <form id="registrationForm" class="form" action="##" method="post">
      <div class="form-group">

        <div class="col-xs-6">
          <label for="first_name"><h4>First name</h4></label>
          <input id="first_name" type="text" class="form-control" name="first_name" placeholder="first name" title="enter your first name if any.">
        </div>
      </div>
      <div class="form-group">

        <div class="col-xs-6">
          <label for="last_name"><h4>Last name</h4></label>
          <input id="last_name" type="text" class="form-control" name="last_name" placeholder="last name" title="enter your last name if any.">
        </div>
      </div>

      <div class="form-group">

        <div class="col-xs-6">
          <label for="phone"><h4>Phone</h4></label>
          <input id="phone" type="text" class="form-control" name="phone" placeholder="enter phone" title="enter your phone number if any.">
        </div>
      </div>

      <div class="form-group">
        <div class="col-xs-6">
          <label for="mobile"><h4>Mobile</h4></label>
          <input id="mobile" type="text" class="form-control" name="mobile" placeholder="enter mobile number" title="enter your mobile number if any.">
        </div>
      </div>
      <div class="form-group">

        <div class="col-xs-6">
          <label for="email"><h4>Email</h4></label>
          <input id="email" type="email" class="form-control" name="email" placeholder="you@email.com" title="enter your email.">
        </div>
      </div>
      <div class="form-group">

        <div class="col-xs-6">
          <label for="email"><h4>Location</h4></label>
          <input id="location" type="email" class="form-control" placeholder="somewhere" title="enter a location">
        </div>
      </div>
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

  </div><!--/tab-pane-->
</template>

<script>
import { fetchList } from '@/api/article'

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
      loading: false
    }
  },
  created() {
    this.getList()
  },
  methods: {
    getList() {
      this.loading = true
      this.$emit('create') // for test
      fetchList(this.listQuery).then(response => {
        this.list = response.data.items
        this.loading = false
      })
    }
  }
}
</script>

