import React from 'react'
import View from './Components/View'
import Form from './Components/Form'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import Navbar from './Components/Navbar'

const App = () => {
  return (
   <BrowserRouter className="App">
   <Navbar />
  <Routes>
   <Route exact path="/" element={<View />} />
   <Route exact path="/data" element={<Form/>}/>
  </Routes>
 </BrowserRouter>
  )
}

export default App
