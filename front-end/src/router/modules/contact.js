/** When your routing table is too long, you can split it into small modules**/

import Layout from '@/views/layout/Layout';

const contactRouter = {
  path: '/contact',
  component: Layout,
  redirect: '/contact/list',
  name: 'contact',
  meta: {
    // title: 'position',
    icon: 'example'
  },
  children: [
    {
      path: 'list',
      component: () => import('@/views/contact/index'),
      name: 'Contact',
      meta: {
        title: 'contact',
        roles: ['SuperAdmin', 'HR'] // or you can only set roles in sub nav
      }
    }
  ]
};
export default contactRouter;
