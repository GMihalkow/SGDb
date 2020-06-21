import axios from 'axios';

const client = axios.create({
    timeout: 10000, // indicates, 1000ms ie. 1 second
    headers: {
        'Content-Type': 'application/json',
        'Access-Control-Allow-Headers' : '*'
    }
});

export default {
    postWithFormData(url, data) { 
        return client.post(url, data, {
            headers: {
                'Content-Type': 'multipart/form-data; boundary=' + data._boundary,
                'Authorization': axios.defaults.headers.common['Authorization'] ? axios.defaults.headers.common['Authorization'] : ''
            }
        });
    },
    get(url) {
        return client.get(url, { 
            headers:  {
                'Authorization': axios.defaults.headers.common['Authorization'] ? axios.defaults.headers.common['Authorization'] : ''
            }
        });
    }
};