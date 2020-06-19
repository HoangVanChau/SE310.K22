<template>
  <div class="app-container">
    <el-row>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.user') }}</p>
          </el-col>
          <el-col :span="16">{{ data.userId || 'Trống' }}</el-col>
        </div>
      </el-col>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.baseSalary') }}</p>
          </el-col>
          <el-col :span="16">{{ data.baseSalary || 'Trống' }}</el-col>
        </div
      ></el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.year') }}</p>
          </el-col>
          <el-col :span="16">{{ data.year || 'Trống' }}</el-col>
        </div>
      </el-col>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.month') }}</p>
          </el-col>
          <el-col :span="16">{{ data.month || 'Trống' }}</el-col>
        </div>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.bonus') }}</p>
          </el-col>
          <el-col :span="16">{{ data.bonus || 'Trống' }}</el-col>
        </div>
      </el-col>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.social') }}</p>
          </el-col>
          <el-col :span="16">{{ data.socialInsurance || 'Trống' }}</el-col>
        </div>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.personal') }}</p>
          </el-col>
          <el-col :span="16">{{ data.personalIncomeTax || 'Trống' }}</el-col>
        </div>
      </el-col>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.totalExpectWorkingHours') }}</p>
          </el-col>
          <el-col :span="16">{{ data.totalExpectWorkingHours || 'Trống' }}</el-col>
        </div>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.totalHours') }}</p>
          </el-col>
          <el-col :span="16">{{ data.totalWorkingHours || 'Trống' }}</el-col>
        </div>
      </el-col>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.paid') }}</p>
          </el-col>
          <el-col :span="16">{{ data.paidLeaveHours || 'Trống' }}</el-col>
        </div>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.total') }}</p>
          </el-col>
          <el-col :span="16">{{ data.totalSalary || 'Trống' }}</el-col>
        </div>
      </el-col>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.totalPaid') }}</p>
          </el-col>
          <el-col :span="16">{{ data.paidLeaveTotal || 'Trống' }}</el-col>
        </div>
      </el-col>
    </el-row>
    <el-row>
      <el-col :span="12">
        <div class="grid-content">
          <el-col :span="8">
            <p class="field">{{ $t('payRoll.final') }}</p>
          </el-col>
          <el-col :span="16">{{ data.finalReceiveSalary || 'Trống' }}</el-col>
        </div>
      </el-col>
    </el-row>
    <el-row :gutter="20">
      <el-col :span="6" :offset="6">
        <el-button type="primary" round>Xuất file PDF</el-button>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'PayRoll',
  data() {
    return {
      data: null
    }
  },
  computed: {
    ...mapGetters([
      'curUser',
    ])
  },
  created() {
    this.getCurPayRoll()
  },
  methods: {
    getCurPayRoll() {
      this.$store.dispatch('GetPayRollByUser', this.curUser.userId).then(res => {
        if (res) {
          this.data = res;
        }
      })
    }
  },

}
</script>

<style lang="scss">
.field{
  font-weight: bold;
  font-size: 2rem;
}
.grid-content{
  display: flex;
  width: 100%;
  font-size: 1.5rem;
}

</style>>
