import { useState, useEffect } from "react";
import "./App.css";

function App() {
  const [policies, setPolicies] = useState([]);
  const [selectedPolicyId, setSelectedPolicyId] = useState("");
  const [newPolicyTitle, setNewPolicyTitle] = useState("");
  const [newPolicyRule, setNewPolicyRule] = useState("");
  const [simulationResult, setSimulationResult] = useState(null);
  const [loading, setLoading] = useState(false);

  // Matches your backend port 5068 and controller routes
  const API_BASE = "http://localhost:5068/api";

  useEffect(() => {
    fetchPolicies();
  }, []);

  const fetchPolicies = async () => {
    try {
      const res = await fetch(`${API_BASE}/policies`);
      if (!res.ok) throw new Error("Failed to fetch policies");
      const data = await res.json();
      setPolicies(data);
      if (data.length > 0) setSelectedPolicyId(data[0].id);
    } catch (err) {
      console.error("Failed to fetch policies", err);
    }
  };

  const handleCreatePolicy = async (e) => {
    e.preventDefault();
    if (!newPolicyTitle || !newPolicyRule) return;

    try {
      const res = await fetch(`${API_BASE}/policies`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          policyTitle: newPolicyTitle,
          naturalLanguageRule: newPolicyRule,
        }),
      });
      if (!res.ok) throw new Error("Failed to create policy");
      const created = await res.json();
      setPolicies([...policies, created]);
      setSelectedPolicyId(created.id);
      setNewPolicyTitle("");
      setNewPolicyRule("");
    } catch (err) {
      console.error("Failed to create policy", err);
    }
  };

  const handleRunSimulation = async () => {
    if (!selectedPolicyId) return;
    setLoading(true);
    try {
      const res = await fetch(
        `${API_BASE}/simulation/run/${selectedPolicyId}`,
        {
          method: "POST",
        },
      );
      if (!res.ok) throw new Error("Simulation failed");
      const result = await res.json();
      setSimulationResult(result);
    } catch (err) {
      console.error("Simulation failed", err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className="container"
      style={{
        padding: "2rem",
        maxWidth: "900px",
        margin: "0 auto",
        fontFamily: "sans-serif",
        textAlign: "left",
      }}
    >
      <h1>TaxSim-Agent Sandbox</h1>
      <p style={{ color: "#666" }}>
        Autonomous Multi-Agent Policy & Compliance Simulation Engine
      </p>

      {/* Policy Creator Section */}
      <div
        style={{
          background: "#f4f4f4",
          padding: "1.5rem",
          borderRadius: "8px",
          marginBottom: "2rem",
        }}
      >
        <h3>Draft New Tax Policy</h3>
        <form
          onSubmit={handleCreatePolicy}
          style={{ display: "flex", flexDirection: "column", gap: "10px" }}
        >
          <input
            type="text"
            placeholder="Policy Title (e.g. 25% Tax Rule)"
            value={newPolicyTitle}
            onChange={(e) => setNewPolicyTitle(e.target.value)}
            style={{
              padding: "8px",
              borderRadius: "4px",
              border: "1px solid #ccc",
            }}
          />
          <textarea
            placeholder="Natural language rule description..."
            value={newPolicyRule}
            onChange={(e) => setNewPolicyRule(e.target.value)}
            style={{
              padding: "8px",
              borderRadius: "4px",
              border: "1px solid #ccc",
              minHeight: "80px",
            }}
          />
          <button
            type="submit"
            style={{
              background: "#007bff",
              color: "white",
              padding: "10px",
              border: "none",
              borderRadius: "4px",
              cursor: "pointer",
            }}
          >
            Save Policy
          </button>
        </form>
      </div>

      {/* Simulation Runner Section */}
      <div
        style={{
          background: "#eef2f7",
          padding: "1.5rem",
          borderRadius: "8px",
          marginBottom: "2rem",
        }}
      >
        <h3>Run Simulation</h3>
        <div style={{ display: "flex", gap: "10px", alignItems: "center" }}>
          <select
            value={selectedPolicyId}
            onChange={(e) => setSelectedPolicyId(e.target.value)}
            style={{
              padding: "8px",
              borderRadius: "4px",
              border: "1px solid #ccc",
              flex: 1,
            }}
          >
            {policies.map((p) => (
              <option key={p.id} value={p.id}>
                {p.policyTitle}: {p.naturalLanguageRule?.substring(0, 60)}...
              </option>
            ))}
          </select>
          <button
            onClick={handleRunSimulation}
            disabled={loading}
            style={{
              background: "#28a745",
              color: "white",
              padding: "10px 20px",
              border: "none",
              borderRadius: "4px",
              cursor: "pointer",
            }}
          >
            {loading ? "Simulating Swarm..." : "Run Simulation"}
          </button>
        </div>
      </div>

      {/* Results View */}
      {simulationResult && (
        <div
          style={{
            background: "#fff",
            border: "1px solid #ddd",
            padding: "1.5rem",
            borderRadius: "8px",
          }}
        >
          <h3>Simulation Report (Run #{simulationResult.id})</h3>
          <div style={{ display: "flex", gap: "20px", margin: "15px 0" }}>
            <div
              style={{
                background: "#e8f5e9",
                padding: "10px 15px",
                borderRadius: "6px",
                flex: 1,
              }}
            >
              <strong>Total Revenue:</strong> $
              {simulationResult.totalRevenueCollected?.toLocaleString()}
            </div>
            <div
              style={{
                background: "#ffebee",
                padding: "10px 15px",
                borderRadius: "6px",
                flex: 1,
              }}
            >
              <strong>Compliance Failures:</strong>{" "}
              {simulationResult.complianceFailureCount}
            </div>
          </div>

          <h4>Agent Audit Logs</h4>
          <div
            style={{ display: "flex", flexDirection: "column", gap: "15px" }}
          >
            {simulationResult.auditLogs?.map((log) => (
              <div
                key={log.id}
                style={{
                  border: "1px solid #eee",
                  padding: "12px",
                  borderRadius: "6px",
                  background: "#fafafa",
                }}
              >
                <div
                  style={{
                    display: "flex",
                    justifyContent: "space-between",
                    marginBottom: "5px",
                  }}
                >
                  <strong>
                    {log.taxAgent?.agentName} ({log.taxAgent?.behavioralProfile}
                    )
                  </strong>
                  <span
                    style={{
                      color: log.riskFlag?.includes("High Risk")
                        ? "#d32f2f"
                        : "#388e3c",
                      fontWeight: "bold",
                    }}
                  >
                    {log.riskFlag}
                  </span>
                </div>
                <p style={{ margin: "5px 0", fontSize: "14px", color: "#333" }}>
                  {log.actionTaken}
                </p>
                <small style={{ color: "#777" }}>
                  Income: ${log.taxAgent?.income?.toLocaleString()}
                </small>
              </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}

export default App;
