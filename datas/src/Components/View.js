import React, { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

const View = () => {
  const [data, setData] = useState([]);

  useEffect(() => {
    fetchData();
  }, [data]);

  const fetchData = () => {
    axios
      .get("https://localhost:7120/api/data")
      .then((res) => setData(res.data))
      .catch((error) => console.log(error));
  };

  const handleDelete = (batchId) => {
    if (window.confirm(`Are you sure you want to delete batch ${batchId}?`)) {
      axios
        .delete(`https://localhost:7120/api/batch/${batchId}`)
        .then(() => {
          setData((prevData) =>
            prevData.filter((item) => item.batch !== batchId)
          );
          toast.success("Successfully deleted");
        })
        .catch((error) => {
          toast.error("Cannot delete now!");
          console.log(error);
        });
    } else {
      console.log("You canceled");
    }
  };

  const groupedData = data.reduce((acc, item) => {
    if (!acc[item.batch]) {
      acc[item.batch] = [];
    }
    acc[item.batch].push(item);
    return acc;
  }, {});

  return (
    <div style={{ margin: "auto", maxWidth: "600px" }}>
      <h1 style={{ textAlign: "center", marginBottom: "20px" }}>User Data</h1>

      {Object.entries(groupedData).length === 0 ? (
        <>
          <hr />
          <h2 style={{ textAlign: "center", marginBottom: "20px" }}>No data</h2>
        </>
      ) : (
        <>
          {Object.entries(groupedData)
            .sort((a, b) => b[0] - a[0])
            .map(([batchId, batchData]) => (
              <div key={batchId} style={{ marginBottom: "30px" }}>
                <div className="d-flex flex-row">
                  <h3>Batch: {batchId}</h3>
                  <td className="mx-5">
                    <a
                      className="btn btn-primary"
                      // onClick={() => editBatch(batchId)}
                    >
                      Edit
                    </a>
                  </td>
                  <td>
                    <button
                      className="btn btn-danger"
                      onClick={() => handleDelete(batchId)}
                    >
                      Delete
                    </button>
                  </td>
                </div>

                <table className="table">
                  <thead>
                    <tr>
                      <th>ID</th>
                      <th>Name</th>
                      <th>Gender</th>
                      <th>Hobbies</th>
                    </tr>
                  </thead>
                  <tbody>
                    {batchData.map((item) => (
                      <tr key={item.id}>
                        <td>{item.id}</td>
                        <td>{item.name}</td>
                        <td>{item.gender}</td>
                        <td>{item.hobbies.join(", ")}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            ))}
        </>
      )}
    </div>
  );
};

export default View;
