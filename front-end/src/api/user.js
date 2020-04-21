import request from '@/utils/request';

export function getAllUsers() {
  return request({
    url: '/user/getAllUsers',
    method: 'get'
  });
}
export function updateUser(id, data) {
  return request({
    url: `/api/user/${id}`,
    method: 'patch',
    data
  });
}
export function deleteUser(id) {
  return request({
    url: `/api/user/${id}`,
    method: 'delete'
  });
}
export function addUser(data) {
  return request({
    url: `/api/auth/register`,
    method: 'post',
    data
  });
}
export function uploadAvatar(file) {
  return new Promise(function(resolve, reject) {
    const CLOUDINARY_URL = 'https://api.cloudinary.com/v1_1/critplease/upload';
    const CLOUDINARY_UPLOAD_PRESET = 'cqj4erwy';

    var formData = new FormData();
    formData.append('upload_preset', CLOUDINARY_UPLOAD_PRESET);
    formData.append('folder', 'cloudinary-demo');
    formData.append('file', file);

    var request = new XMLHttpRequest();
    request.open('POST', CLOUDINARY_URL, true);
    request.setRequestHeader('X-Requested-With', 'XMLHttpRequest');

    request.onreadystatechange = () => {
      var response;
      // File uploaded successfully
      if (request.readyState === 4 && request.status === 200) {
        response = JSON.parse(request.responseText);
        resolve(response);
      }

      // Not succesfull, let find our what happened
      if (request.status !== 200) {
        response = JSON.parse(request.responseText);
        var error = response.error.message;
        alert('error, status code not 200 ' + error);
        reject(error);
      }
    };

    request.onerror = err => {
      alert('error: ' + err);
      reject(err);
    };

    request.send(formData);
  });
}
