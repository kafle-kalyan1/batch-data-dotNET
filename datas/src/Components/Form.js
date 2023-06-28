import React, { useState } from 'react';
import { useFormik } from 'formik';
import axios from 'axios';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const Form = () => {
  const [dataArr, setDataArr] = useState([]);
  const [batchId, setBatchId] = useState(null);

  const validate = (values) => {
    const errors = {};

    if (!values.name) {
      errors.name = 'Name is required';
    }

    if (!values.gender) {
      errors.gender = 'Gender is required';
    }

    if (values.hobbies.length === 0) {
      errors.hobbies = 'Select at least one hobby';
    }

    return errors;
  };

  const formik = useFormik({
    initialValues: {
      name: '',
      gender: '',
      hobbies: [],
    },
    validate,
    onSubmit: (values, actions) => {
      if (!batchId) {
        // Create a new batch
        axios
          .post('http://localhost:5173/api/batch')
          .then((res) => {
            const { id } = res.data;
            setBatchId(id);
            addToTable(values, id);
            actions.resetForm();
            toast.success('Batch created successfully');
          })
          .catch((err) => {
            toast.error('Failed to create batch');
          });
      } else {
        // Add data to existing batch
        addToTable(values, batchId);
        actions.resetForm();
        toast.success('Item added successfully');
      }
      addToTable(values, batchId);

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

    formik.setFieldValue('hobbies', newHobby);
  };

  const addToTable = (item, batchId) => {
    setDataArr((prevData) => [...prevData, { ...item, batch: batchId }]);
  };

  const handleSaveBatch = () => {
    if (dataArr.length === 0) {
      toast.error('No data to save');
      return;
    }

    axios
      .post('http://localhost:5173/api/batch/save', dataArr)
      .then(() => {
        setBatchId(null);
        setDataArr([]);
        toast.success('Batch saved successfully');
      })
      .catch((err) => {
        toast.error('Failed to save batch');
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
              {formik.errors.name && formik.touched.name && (
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
              {formik.errors.gender && formik.touched.gender && (
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
                    checked={formik.values.hobbies.includes('reading')}
                    onChange={() => handleCheckboxChange('reading')}
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
                    checked={formik.values.hobbies.includes('eating')}
                    onChange={() => handleCheckboxChange('eating')}
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
                    checked={formik.values.hobbies.includes('dance')}
                    onChange={() => handleCheckboxChange('dance')}
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
                    checked={formik.values.hobbies.includes('gaming')}
                    onChange={() => handleCheckboxChange('gaming')}
                  />
                  Gaming
                </label>
              </div>
              {formik.errors.hobbies && formik.touched.hobbies && (
                <div className="text-danger">{formik.errors.hobbies}</div>
              )}
            </div>

            <button type="button" className="btn btn-primary" onClick={formik.handleSubmit}>
              Add
            </button>
          </form>

          {dataArr.length === 0 ? (
            <h1>No data</h1>
          ) : (
            <div>
              {dataArr.map((res, index) => (
                <div key={index}>
                  <p>{res.name}</p>
                  <p>{res.gender}</p>
                  <p>{res.hobbies.join(', ')}</p>
                </div>
              ))}
              <button
                type="button"
                className="btn btn-success"
                onClick={handleSaveBatch}
              >
                Save Batch
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Form;
