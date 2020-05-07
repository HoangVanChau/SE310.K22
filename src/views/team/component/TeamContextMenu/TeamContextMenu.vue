<template>
  <div id="contextmenu" class="contextmenu">
    <div class="contextmenu__item" @click="handleAdd()">Thêm vào nhóm</div>
    <div class="contextmenu__item" @click="handleRemove()">Xóa khỏi nhóm</div>
  </div>
</template>

<script>
export default {
  name: 'TeamContextMenu',
  data() {
    return {
      row: null
    }
  },
  methods: {
    init(row, column, event) {
      // 设置菜单出现的位置
      // 具体显示位置根据自己需求进行调节
      const menu = document.querySelector('#contextmenu')
      const cha = document.body.clientHeight - event.clientY
      console.log(document.body.clientHeight, event.clientY, cha)
      // 防止菜单太靠底，根据可视高度调整菜单出现位置
      if (cha < 150) {
        menu.style.top = event.clientY - 120 + 'px'
      } else {
        menu.style.top = event.clientY - 10 + 'px'
      }
      menu.style.left = event.clientX + 10 + 'px'
      document.addEventListener('click', this.onClose) // 给整个document添加监听鼠标事件，点击任何位置执行foo方法
      this.row = row;
    },
    onClose() {
      this.$emit('close')
    },
    handleAdd() {
      this.$emit('handleAdd', this.row)
    },
    handleRemove() {
      this.$emit('handleRemove', this.row)
    },
  }
}
</script>

<style scoped>
  .contextmenu__item {
    display: block;
    line-height: 34px;
    text-align: center;
  }
  .contextmenu__item:not(:last-child) {
    border-bottom: 1px solid rgba(64,158,255,.2);
  }
  .contextmenu {
    position: absolute;
    background-color: #ecf5ff;
    width: 100px;
    /*height: 106px;*/
    font-size: 12px;
    color: #409EFF;
    border-radius: 4px;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    border: 1px solid rgba(64,158,255,.2);
    white-space: nowrap;
    z-index: 1000;
  }
  .contextmenu__item:hover {
    cursor: pointer;
    background: #66b1ff;
    border-color: #66b1ff;
    color: #fff;
  }
</style>
