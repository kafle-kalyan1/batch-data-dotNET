import React, { useEffect, useState } from "react";
import { useFormik } from "formik";
import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const Form = (datass) => {
  const [dataArr, setDataArr] = useState([]);
  // if(datass){
  //   useEffect(()=>{
  //     axios.get("https://localhost:7120/api/data").then((res)=>{
  //       console.log(res)
  //     // res.data.filter(()=>{
  
  //     // })
  //     })
  //     setDataArr(datass)
  //   })
    
  // }
  useEffect(() => {
    
  }, [setDataArr])
  

  const validate = (values) => {
    const errors = {};

    if (!values.name) {
      errors.name = "Name is required";
    }

    if (!values.gender) {
      errors.gender = "Gender is required";
    }

    if (values.hobbies.length === 0) {
      errors.hobbies = "Select at least one hobby";
    }

    return errors;
  };
  function editNow(index, res){
    formik.setValues({
      name: res.name,
      gender: res.gender,
      hobbies: res.hobbies
     });
     setDataArr(prevData => {
      const newDataArr = [...prevData];
      newDataArr.splice(index, 1);
      return newDataArr;
    });
  }
  function deleteNow(index){
if(window.confirm("Are you sure?")){
  setDataArr(prevData => {
    const newDataArr = [...prevData];
    newDataArr.splice(index, 1);
    return newDataArr;
  });
}
  }

  const formik = useFormik({
    initialValues: {
      name: "",
      gender: "",
      hobbies: [],
    },
    validate,
    onSubmit: (values) => {
      addToTable(values);
    },
  });

  const handleCheckboxChange = (value) => {
    const { hobbies } = formik.values;
    const newHobby = [...hobbies];

    if (newHobby.includes(value)) {
      newHobby.splice(newHobby.indexOf(value), 1);
    } else {
      newHobby.push(value);
    }

    formik.setFieldValue("hobbies", newHobby);
  };

  const addToTable = (item) => {
    setDataArr((prevData) => [...prevData, item]);
    formik.setValues({ name: "", gender: "", hobbies: [] });
  };
  

  function deleteBatch(batchId) {
    axios
      .delete(`https://localhost:7120/api/batch/${batchId}`)
      .then((res) => {
        toast.error("Batch Deleted");
      })
      .catch((error) => {
        toast.error("Some error");
      });
  }

  const handleSaveBatch = () => {
    if (dataArr.length === 0) {
      toast.error("No data to save");
      return;
    }

    try {
      axios
        .post("https://localhost:7120/api/batch")
        .then((res) => {
          let batchId = res.data.id;
          console.log("oikkk");
          const requestData = dataArr.map((data) => {
            return {
              name: data.name,
              gender: data.gender,
              hobbies: data.hobbies,
              batch: batchId,
            };
          });

          toast.success("Batch saved successfully");
          addDataToDB(requestData, batchId);
        })
        .catch(() => {
          toast.error("Batch saved ERROR");
        });
      function addDataToDB(requestData, batchId) {
        console.log(requestData);
        axios
          .post("https://localhost:7120/api/data/add", requestData)
          .then(() => {
            console.log(requestData);
            setDataArr([]);
            toast.success("Data saved successfully");
          })
          .catch(() => {
            console.log(requestData);
            deleteBatch(batchId);
          });
      }
    } catch (error) {
      toast.error("Failed to save batch: " + error.message);
    }
  };

  return (
    <div className="container">
      <div className="row">
        <div className="col-md-6 offset-md-3">
          <form onSubmit={formik.handleSubmit}>
            <div className="form-group">
              <label htmlFor="name">Name</label>
              <input
                id="name"
                name="name"
                placeholder="Please enter your name"
                type="text"
                className="form-control"
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
                value={formik.values.name}
              />
              {formik.errors.name && formik.touched.name && !formik.isSubmitting && (
                <div className="text-danger">{formik.errors.name}</div>
              )}
            </div>

            <div className="form-group">
              <label htmlFor="gender">Gender</label>
              <select
                id="gender"
                name="gender"
                className="form-control"
                value={formik.values.gender}
                onChange={formik.handleChange}
                onBlur={formik.handleBlur}
              >
                <option value="">Select a gender</option>
                <option value="male">Male</option>
                <option value="female">Female</option>
              </select>
              {formik.errors.gender && formik.touched.gender && !formik.isSubmitting && (
                <div className="text-danger">{formik.errors.gender}</div>
              )}
            </div>

            <div className="form-group">
              <div>Hobbies</div>
              <div>
                <label>
                  <input
                    type="checkbox"
                    name="hobbies"
                    value="reading"
                    checked={formik.values.hobbies.includes("reading")}
                    onChange={() => handleCheckboxChange("reading")}
                  />
                  Reading
                </label>
              </div>
              <div>
                <label>
                  <input
                    type="checkbox"
                    name="hobbies"
                    value="eating"
                    checked={formik.values.hobbies.includes("eating")}
                    onChange={() => handleCheckboxChange("eating")}
                  />
                  Eating
                </label>
              </div>
              <div>
                <label>
                  <input
                    type="checkbox"
                    name="hobbies"
                    value="dance"
                    checked={formik.values.hobbies.includes("dance")}
                    onChange={() => handleCheckboxChange("dance")}
                  />
                  Dance
                </label>
              </div>
              <div>
                <label>
                  <input
                    type="checkbox"
                    name="hobbies"
                    value="gaming"
                    checked={formik.values.hobbies.includes("gaming")}
                    onChange={() => handleCheckboxChange("gaming")}
                  />
                  Gaming
                </label>
              </div>
              {formik.errors.hobbies && !formik.isSubmitting && formik.touched.hobbies && (
                <div className="text-danger">{formik.errors.hobbies}</div>
              )}
            </div>

            <button type="submit" className="btn btn-primary">
              Add
            </button>
          </form>

          <table className="table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Gender</th>
                <th>Hobbies</th>
              </tr>
            </thead>
            {dataArr.length === 0 ? (
              <tbody>
                <tr>
                  <td colSpan="3" className="text-center">No data</td>
                </tr>
              </tbody>
            ) : (
              <>
                <tbody>
                  {dataArr.map((res, index) => (
                    <tr key={index}>
                      <td>{res.name}</td>
                      <td>{res.gender}</td>
                      <td>{res.hobbies.join(", ")}</td>
                      <td><button className="btn btn-primary" onClick={()=> editNow(index, res)}>Edit</button></td>
                      <td><button className="btn btn-danger" onClick={()=> deleteNow(index)}>Delete</button></td>
                    </tr>
                  ))}
                </tbody>
                <button
                  type="button"
                  className="btn btn-success"
                  onClick={handleSaveBatch}
                >
                  Save Batch
                </button>
              </>
            )}
          </table>
        </div>
      </div>
    </div>
  );
};

export default Form;
