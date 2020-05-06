/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout'

const departmentRouter = {
  path: '/department',
  component: Layout,
  redirect: '/department/list',
  name: 'Department',
  meta: {
    title: 'Department',
    icon: 'chart'
  },
  children: [
    {
      path: 'list',
      component: () => import('@/views/department/index'),
      name: 'LstDepartment',
      meta: { title: 'lstDepartment' }
    }
  ]
}
export default departmentRouter
