/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout';

const dateOffRouter = {
  path: '/off',
  component: Layout,
  redirect: '/off/submit',
  name: 'Date Off',
  roles: ['SuperAdmin', 'Manager'],
  meta: {
    title: 'dateOff',
    icon: 'clipboard'
  },
  children: [
    {
      path: 'approve',
      component: () => import('@/views/DateOff/approve'),
      name: 'Approve',
      meta: {
        title: 'approveDateOff',
        roles: ['SuperAdmin', 'Manager'] // or you can only set roles in sub nav
      }
    },
    {
      path: 'submit',
      component: () => import('@/views/DateOff/index'),
      name: 'Submit',
      meta: {
        title: 'submitDateOff',
        roles: ['SuperAdmin', 'Employee', 'Manager'] // or you can only set roles in sub nav
      }
    }
  ]
};
export default dateOffRouter;
