/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout'

const tableRouter = {
  path: '/table',
  component: Layout,
  redirect: '/table/complex-table',
  name: 'Table',
  meta: {
    title: 'Table',
    icon: 'table'
  },
  children: [
    {
      path: 'dynamic-table',
      component: () => import('@/views/table/dynamicTable/index'),
      name: 'DynamicTable',
      meta: { title: 'dynamicTable' }
    },
    {
      path: 'drag-table',
      component: () => import('@/views/table/dragTable'),
      name: 'DragTable',
      meta: { title: 'dragTable' }
    },
    {
      path: 'team',
      component: () => import('@/views/team/index'),
      name: 'Team',
      meta: { title: 'team' }
    },
    {
      path: 'user',
      component: () => import('@/views/user/index'),
      name: 'User',
      meta: { title: 'user' }
    },
    {
      path: 'custom-tree-table',
      component: () => import('@/views/table/treeTable/customTreeTable'),
      name: 'CustomTreeTableDemo',
      meta: { title: 'customTreeTable' }
    },
    {
      path: 'complex-table',
      component: () => import('@/views/table/complexTable'),
      name: 'ComplexTable',
      meta: { title: 'complexTable' }
    }
  ]
}
export default tableRouter
