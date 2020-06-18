/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout';

const holidayRouter = {
  path: '/holiday',
  component: Layout,
  redirect: '/holiday/list',
  name: 'holiday',
  meta: {
    // title: 'position',
    icon: 'bug',
  },
  children: [
    {
      path: 'list',
      component: () => import('@/views/holiday/index'),
      name: 'Holiday',
      meta: {
        title: 'holiday',
      }
    }
  ]
};
export default holidayRouter;
