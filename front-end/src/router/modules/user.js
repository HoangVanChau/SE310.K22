/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout';

const userRouter = {
  path: '/user',
  component: Layout,
  redirect: '/user/list',
  name: 'User',
  meta: {
    title: 'Employee',
    icon: 'documentation'
  },
  children: [
    {
      path: 'list',
      component: () => import('@/views/user/index'),
      name: 'LstUser',
      meta: { title: 'lstUser' }
    },
    {
      path: 'upload-excel',
      component: () => import('@/views/excel/uploadExcel'),
      name: 'UploadExcel',
      meta: {
        title: 'timeKeeping',
        roles: ['SuperAdmin', 'Manager'] // or you can only set roles in sub nav
      }
    }
  ]
};
export default userRouter;
