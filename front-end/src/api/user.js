import request from '@/utils/request';

export function getAllUsers() {
  return request({
    url: '/api/users/getAllUser',
    method: 'get'
  });
}
export function getCurUser() {
  return request({
    url: '/api/auth/verify',
    method: 'get'
  });
}
export function getUser(userId) {
  return request({
    url: '/api/users/'.concat(userId),
    method: 'get'
  });
}
export function updateCurUser(dataParam) {
  const data = {
    UserName: dataParam.userName,
    FullName: dataParam.fullName,
    PhoneNumber: dataParam.phoneNumber,
    Email: dataParam.email,
    DateOfBirth: dataParam.dateOfBirth,
    Address: {
      Province: {
        LocateId: dataParam.address.province.locateId || null,
        LocateName: dataParam.address.province.locateName || null
      },
      District: {
        LocateId: dataParam.address.district.locateId || null,
        LocateName: dataParam.address.district.locateName || null
      },
      Ward: {
        LocateId: dataParam.address.ward.locateId || null,
        LocateName: dataParam.address.ward.locateName || null
      },
      DetailAddress: dataParam.address.detailAddress || null
    },
    AvatarImageId: dataParam.avatarImageId || null
  };
  return request({
    url: '/api/users',
    method: 'patch',
    data
  });
}

export function createUser(dataParam) {
  const data = {
    UserName: dataParam.userName,
    FullName: dataParam.fullName,
    Password: '123456789',
    PhoneNumber: dataParam.phoneNumber,
    Email: dataParam.email,
    DateOfBirth: dataParam.dateOfBirth,
    Address: {
      Province: null,
      District: null,
      Ward: null
    },
    AvatarImageId: null
  };
  return request({
    url: '/api/users',
    method: 'post',
    data
  });
}

export function updateUser(userId, dataParam) {
  const data = {
    UserName: dataParam.userName,
    FullName: dataParam.fullName,
    PhoneNumber: dataParam.phoneNumber,
    Email: dataParam.email,
    DateOfBirth: dataParam.dateOfBirth,
    Address: {
      Province: dataParam.province || null,
      District: dataParam.district || null,
      Ward: dataParam.ward || null,
      DetailAddress: dataParam.detailAddress || null
    },
    AvatarImageId: dataParam.avatarImageId || null
  };
  return request({
    url: '/api/users/'.concat(userId),
    method: 'patch',
    data
  });
}
export function deleteUser(userId) {
  return request({
    url: '/api/users/'.concat(userId),
    method: 'delete'
  });
}
export function changePassword(dataParam) {
  const data = {
    OldPassword: dataParam.oldPassword,
    NewPassword: dataParam.newPassword
  };
  return request({
    url: '/api/auth/password',
    method: 'patch',
    data
  });
}
