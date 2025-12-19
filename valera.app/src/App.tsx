// src/App.tsx (фрагмент)
import { useState } from "react";
import "./styles.css";
import { AuthForm } from "./component/AuthForm";
import { getToken, setToken } from "./api";
import { ValeraList } from "./component/ValeraList";
import { ValeraDetails } from "./component/ValeraDetails";
import type { ValeraDto } from "./types";

export default function App() {
    const [current, setCurrent] = useState<ValeraDto | null>(null);
    const [listReloadFlag, setListReloadFlag] = useState(0);
    const [email, setEmail] = useState<string | null>(null);
    const authed = !!getToken();

    const logout = () => { setToken(null); setEmail(null); setCurrent(null); setListReloadFlag(x => x + 1); };

    return (
        <div className="container vstack">
            <h1 className="title">Valera Manager</h1>
            <div className="hstack" style={{ justifyContent: "space-between", marginBottom: 8 }}>
                {authed
                    ? <div className="hstack"><span className="badge">{email ?? "Вы вошли"}</span><button className="btn" onClick={logout}>Выйти</button></div>
                    : null}
            </div>
            <hr className="hr" />
            {!authed ? (
                <AuthForm onAuth={(e) => setEmail(e)} />
            ) : current === null ? (
                <ValeraList key={listReloadFlag} onOpen={(item) => setCurrent(item)} />
            ) : (
                <ValeraDetails
                    initial={current}
                    onBack={() => { setCurrent(null); setListReloadFlag(f => f + 1); }}
                    onRefreshFromList={() => setListReloadFlag(f => f + 1)}
                />
            )}
        </div>
    );
}
