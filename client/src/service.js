import axios from 'axios';


axios.defaults.baseURL = process.env.REACT_APP_API_URL;
axios.interceptors.response.use((response)=>response,(error)=>{
  console.error("API ERROR:",{
     url: error.config?.url,
     method:error.config?.method,
     status:error.response?.status,
    message:error.message,
  })
  return Promise.reject(error)
});
export default {
  getTasks: async () => {
    const result = await axios.get("/items")    
    return result.data;
  },

  addTask: async(name)=>{
    console.log('addTask', name)
    const result = await axios.post("/items",{name,isComplete:false})    
    return result.data;
  },

  setCompleted: async(id, isComplete, name)=>{
    console.log('setCompleted', {id, isComplete})
    const result = await axios.put(`/items/${id}`,{id,name,isComplete})    
    return result.data;
  },

  deleteTask:async(id)=>{
    console.log('deleteTask')
       const result = await axios.delete(`/items/${id}`)    
  }
};
