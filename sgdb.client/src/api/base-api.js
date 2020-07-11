import axios from 'axios';

const client = axios.create({
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Headers' : '*'
    }
});

export default {
    get(url, queryParams) {
        return client.get(url, { 
            headers:  {
                'Authorization': axios.defaults.headers.common['Authorization'] ? axios.defaults.headers.common['Authorization'] : ''
            },
            params: queryParams
        });
    },
    postWithFormData(url, data) { 
        return client.post(url, data, {
            headers: {
                'Content-Type': 'multipart/form-data; boundary=' + data._boundary,
                'Authorization': axios.defaults.headers.common['Authorization'] ? axios.defaults.headers.common['Authorization'] : ''
            }
        });
    },
    putWithFormData(url, data) { 
        return client.put(url, data, {
            headers: {
                'Content-Type': 'multipart/form-data; boundary=' + data._boundary,
                'Authorization': axios.defaults.headers.common['Authorization'] ? axios.defaults.headers.common['Authorization'] : ''
            }
        });
    },
    patch(url, data) {
        return client.patch(url, data, {
            headers: {
                'Authorization': axios.defaults.headers.common['Authorization'] ? axios.defaults.headers.common['Authorization'] : ''
            }
        });      
    },
    delete(url, data) {
        return client.delete(url, {
            headers: {
                'Authorization': axios.defaults.headers.common['Authorization'] ? axios.defaults.headers.common['Authorization'] : ''
            },
            params: data
        });      
    }
};