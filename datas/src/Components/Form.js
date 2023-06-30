import React, { useEffect, useState } from "react";
import { useFormik } from "formik";
import axios from "axios";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";

const Form = () => {
  const [dataArr, setDataArr] = useState([]);
  const [batchId, setBatchId] = useState();
  const [mode, setEditMode] = useState(false);
  const location = useLocation();
  const navigatee = useNavigate();

  useEffect(() => {
    const data = location.state;
    if (data !== null) {
      const dataArray = Object.values(data);
      setDataArr(dataArray);
      setBatchId(dataArray[0].batch);
    }
  }, [ setDataArr, setEditMode]);

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

  function editNow(index, res) {
    formik.setValues({
      id: res.id,
      name: res.name,
      gender: res.gender,
      hobbies: res.hobbies,
    });
    setEditMode(true)
   
  }

  function handleEdit(res){
    setDataArr((prevData) => {
      // Remove the edited item from the data array
      const newDataArr = prevData.filter((item) => item.id !== res.id);
      // Add the updated item to the data array
      newDataArr.push(res);
      return newDataArr;
    });
    formik.setValues({ name: "", gender: "", hobbies: [] });
    setEditMode(false)
  }

  function deleteNow(index) {
    if (window.confirm("Are you sure?")) {
      
      setDataArr((prevData) => {
        const newDataArr = [...prevData];
        newDataArr.splice(index, 1);
        return newDataArr;
      });
          
        }
  }

  function deleteDb(index, id) {
    if (window.confirm("Are you sure?" + id)) {
      axios
        .delete(`https://localhost:7120/api/data/delete/${id}`)
        .then((res) => {
          console.log(res);
          toast.success("Deleted");
          setDataArr((prevData) => {
            const newDataArr = [...prevData];
            newDataArr.splice(index, 1);
            return newDataArr;
          });
        })
        .catch((err) => {
          toast.error(err);
        });
    }
  }
  

  const formik = useFormik({
    initialValues: {
      id:0,
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

  const handleUpdate = (batchId) => {
    if (dataArr.length === 0) {
      toast.error("No data to save");
      return;
    }
    dataArr.forEach((res) => {
      res.batch = batchId;
    });
    axios
      .put(`https://localhost:7120/api/data/edit/${batchId}`, dataArr)
      .then((res) => {
        toast.success("Saved");
        setDataArr(dataArr);
      })
      .catch(() => {
        toast.error("Unable to Update");
      });
  };
  

  const handleSaveBatch = () => {
    if (dataArr.length === 0) {
      toast.error("No data to save");
      return;
    }

    try {
      const data = location.state;
      if (data !== null) {
        let batchId = data[0].id;
        const requestData = dataArr.map((data) => {
          return {
            name: data.name,
            gender: data.gender,
            hobbies: data.hobbies,
            batch: batchId,
          };
        });
        addDataToDB(requestData, batchId);
      } else {
        axios
          .post("https://localhost:7120/api/batch")
          .then((res) => {
            let batchId = res.data.id;

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
      }
    } catch (error) {
      toast.error("Failed to save batch: " + error.message);
    }
  };

  const addDataToDB = (requestData, batchId) => {
    axios
      .post("https://localhost:7120/api/data/add", requestData)
      .then(() => {
        setDataArr([]);
        toast.success("Data saved successfully");
      })
      .catch(() => {
        deleteBatch(batchId);
      });
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
              {formik.errors.name &&
                formik.touched.name &&
                !formik.isSubmitting && (
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
              {formik.errors.gender &&
                formik.touched.gender &&
                !formik.isSubmitting && (
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
              {formik.errors.hobbies &&
                !formik.isSubmitting &&
                formik.touched.hobbies && (
                  <div className="text-danger">{formik.errors.hobbies}</div>
                )}
            </div>

            {mode ? (
              <button
                type="button"
                onClick={()=>handleEdit(formik.values)}
                className="btn btn-primary"
              >
                Edit
              </button>
            ) : (
              <button type="submit" className="btn btn-primary">
                Add
              </button>
            )}
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
                  <td colSpan="3" className="text-center">
                    No data
                  </td>
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
                      <td>
                        <button
                          className="btn btn-primary"
                          onClick={() => editNow(index, res)}
                        >
                          Edit
                        </button>
                      </td>
                      <td>
                        {console.log(res.id)}
                        {res.id !== 0 || undefined ? (
                          <button
                            className="btn btn-danger"
                            onClick={() => deleteDb(index, res.id)}

                            >
                            Delete
                          </button>
                        ) : (
                          <button
                          className="btn btn-danger"
                          onClick={() => deleteNow(index)}
                          >
                            Delete
                          </button>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
                {location.state !== null ? (
                  <button
                    type="button"
                    className="btn btn-success"
                    onClick={() => handleUpdate(batchId)}
                  >
                    Update
                  </button>
                ) : (
                  <button
                    type="button"
                    className="btn btn-success"
                    onClick={handleSaveBatch}
                  >
                    Save Batch
                  </button>
                )}
              </>
            )}
          </table>
        </div>
      </div>
    </div>
  );
};

export default Form;
