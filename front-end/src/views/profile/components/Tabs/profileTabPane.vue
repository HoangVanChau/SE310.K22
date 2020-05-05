<template>
  <div class="pt-0">
    <div v-if="type=='home'" id="home" class="tab-pane active">
      <el-form ref="infoForm" :rules="rules" :model="curUser" class="form border-top-width">
        <div class="col-xs-6">
          <el-form-item prop="fullName">
            <span slot="label" class="fs" >{{ $t('table.fullName') }}</span>
            <el-input v-model="curUser.fullName"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item label-width="100" prop="dateOfBirth">
            <span slot="label" class="fs">{{ $t('table.dateOfBirth') }}</span>
            <el-date-picker
              v-model="curUser.dateOfBirth"
              :placeholder="$t('i18nView.datePlaceholder')"
              type="date"
              format="MM/dd/yyyy"
              value-format="MM/dd/yyyy"
              style="width: 100%"/>
          </el-form-item>
        </div>

        <div class="col-xs-6">
          <el-form-item prop="phoneNumber">
            <span slot="label" class="fs">{{ $t('table.phoneNumber') }}</span>
            <el-input v-model="curUser.phoneNumber"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item prop="userName">
            <span slot="label" class="fs">{{ $t('table.userName') }}</span>
            <el-input v-model="curUser.userName"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item prop="email">
            <span slot="label" class="fs">{{ $t('table.email') }}</span>
            <el-input v-model="curUser.email"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item prop="address">
            <span slot="label" class="fs">{{ $t('table.address') }}</span>
            <el-input v-model="detailAddress"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item prop="province">
            <span slot="label" class="fs">{{ $t('table.province') }}</span>
            <el-input v-model="province"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item prop="district">
            <span slot="label" class="fs">{{ $t('table.district') }}</span>
            <el-input v-model="district"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item prop="ward">
            <span slot="label" class="fs">{{ $t('table.ward') }}</span>
            <el-input v-model="ward"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <br>
          <br>
          <button class="btn btn-lg btn-success mr-5" @click.prevent="saveHomeTab">
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
      </el-form>

      <hr>

    </div><!--/tab-pane-->
    <div v-if="type=='auth'" id="auth">
      <hr>
      <el-form ref="verifyPassForm" :rules="rules" :model="tempPass" class="form">
        <div class="col-xs-6">
          <el-form-item prop="oldpassword">
            <span slot="label" class="fs">{{ $t('table.oldpassword') }}</span>
            <el-input v-model="tempPass.oldpassword" type="password"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item prop="newpassword">
            <span slot="label" class="fs">{{ $t('table.newpassword') }}</span>
            <el-input v-model="tempPass.newpassword" type="password" name="newpassword"/>
          </el-form-item>
        </div>
        <div class="col-xs-6">
          <el-form-item prop="verifypassword">
            <span slot="label" class="fs">{{ $t('table.verifypassword') }}</span>
            <el-input v-model="tempPass.verifypassword" type="password"/>
          </el-form-item>
        </div>
        <div class="form-group">
          <div class="col-xs-12">
            <br>
            <button class="btn btn-lg btn-success" @click.prevent="changePassword"><i
              class="glyphicon glyphicon-ok-sign"
            /> Save</button>
            <button class="btn btn-lg" type="reset"><i class="glyphicon glyphicon-repeat"/> Reset</button>
          </div>
        </div>
      </el-form>

      <hr>
    </div>
  </div>
</template>

<script>
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
      curUser: this.$store.getters.curUser,
      detailAddress: '',
      province: '',
      district: '',
      ward: '',
      tempPass: {
        oldpassword: '',
        newpassword: '',
        verifypassword: '',
      },
      rules: {
        fullName: [{ required: true, message: 'Full name is required', trigger: 'change' }],
        userName: [{ required: true, message: 'User Name is required', trigger: 'blur' }],
        email: [{ required: true, message: 'Email is required', trigger: 'blur' }],
        phoneNumber: [{ required: true, message: 'Phone Number is required', trigger: 'blur' }],
        dateOfBirth: [{ required: true, message: 'Date of birth is required', trigger: 'blur' }],
        oldpassword: [{ required: true, message: 'Old password is required', trigger: 'blur' }],
        verifypassword: [{ required: true, message: 'Verify password is required', trigger: 'blur' }],
        newpassword: [{ required: true, message: 'New password is required', trigger: 'blur' }]
        // validator: (rule, value, callback) => {
        //   if (value !== this.tempPass.verifypassword) {
        //     callback(new Error('Two inputs don\'t match!'))
        //   } else {
        //     callback()
        //   }
        // } }]
      },
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
      this.curUser.address = { }
      this.curUser.address.detailAddress = this.detailAddress
      this.curUser.address.province = this.province
      this.curUser.address.district = this.district
      this.curUser.address.ward = this.ward
      this.$refs['infoForm'].validate((valid) => {
        if (valid) {
          this.$store.dispatch('UpdateCurUser', this.curUser).then(res => {
            if (res) {
              this.$notify({
                title: 'Success',
                message: 'Update successfully',
                type: 'success',
                duration: 2000
              })
            }
          }).catch(e => {
            this.$notify({
              title: 'Error',
              message: 'Update unsuccessfully ' + JSON.stringify(e),
              type: 'error',
              duration: 2000
            })
          });
        }
      })
    },
    changePassword() {
      console.log('this.tempPass.newpassword', this.tempPass.newpassword);
      console.log('this.tempPass.verifypassword', this.tempPass.verifypassword);

      if (this.tempPass.newpassword !== this.tempPass.verifypassword) {
        this.$notify({
          title: 'Error',
          message: 'Verify password does not match with new password',
          type: 'error',
          duration: 2000
        })
        return;
      } else {
        this.$refs['verifyPassForm'].validate((valid) => {
          if (valid) {
            const data = {
              oldPassword: this.tempPass.oldpassword,
              newPassword: this.tempPass.newpassword
            }
            this.$store.dispatch('ChangePassword', data).then(res => {
              if (res) {
                this.$notify({
                  title: 'Success',
                  message: 'Change passoword successfully',
                  type: 'success',
                  duration: 2000
                })
              }
            }).catch(e => {
              this.$notify({
                title: 'Error',
                message: 'Update unsuccessfully ' + JSON.stringify(e),
                type: 'error',
                duration: 2000
              })
            });
          }
        })
      }
    }
  }
}
</script>
<style lang="scss" scoped>
  .fs {
    font-size: 1.8rem;
  }
  /deep/ .el-form-item--medium .el-form-item__content{
    line-height: 28px;
  }
  .mr-5{
    margin-right: 5px;
  }
</style>

