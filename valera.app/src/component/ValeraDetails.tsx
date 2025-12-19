/* eslint-disable @typescript-eslint/no-explicit-any */
import { useState } from "react";
import { ValeraApi } from "../api";
import type { ValeraDto } from "../types";
import { Progress } from "./Progress";

export function ValeraDetails({
  initial,
  onBack,
  onRefreshFromList,
}: {
  initial: ValeraDto;
  onBack: () => void;
  onRefreshFromList: () => void;
}) {
  const [v, setV] = useState<ValeraDto>(initial);
  const [busy, setBusy] = useState(false);
  const [err, setErr] = useState<string | null>(null);

  const act = async (fn: (id: string) => Promise<ValeraDto>) => {
    setBusy(true);
    setErr(null);
    try {
      const res = await fn(v.id);
      setV(res);
      onRefreshFromList(); // чтобы список был в актуальном состоянии
    } catch (e: any) {
      setErr(e.message ?? "Ошибка запроса");
    } finally {
      setBusy(false);
    }
    };

  const vitalityPercent = ((v.vitality + 10) / 20) * 100;
  const canWork = v.tired < 10 && v.mana < 50;

  return (
    <div className="vstack">
      <div className="hstack" style={{justifyContent:'space-between'}}>
        <div className="vstack" style={{gap:4}}>
          <p className="subtitle">ID: {v.id}</p>
        </div>
        <div className="hstack">
          {v.succeeded !== undefined && (
            <span className="badge">{v.succeeded ? "Успешно" : "Не выполнено"}</span>
          )}
          <button className="btn" onClick={onBack}>Назад</button>
        </div>
      </div>

      <div className="grid">
        <div className="card"><Progress label="Здоровье" value={v.health} /></div>
        <div className="card"><Progress label="Мана (алкоголь)" value={v.mana} /></div>
        <div className="card"><Progress label="Жизнерадостность" value={vitalityPercent} /></div>
        <div className="card"><Progress label="Усталость" value={v.tired} /></div>
        <div className="card"><Progress label="Деньги" value={v.money} /></div>
      </div>

      {err && <span className="badge" style={{borderColor:'#ff9b9b', background:'#fff5f5'}}>⚠ {err}</span>}

      <div className="card vstack">
        <div className="hstack" style={{flexWrap:'wrap'}}>
          <button className="btn primary" disabled={!canWork || busy} onClick={() => act(ValeraApi.work)}>Пойти на работу</button>
          <button className="btn" disabled={busy} onClick={() => act(ValeraApi.contemplate)}>Созерцать природу</button>
          <button className="btn" disabled={busy} onClick={() => act(ValeraApi.wineTv)}>Пить вино и смотреть сериал</button>
          <button className="btn" disabled={busy} onClick={() => act(ValeraApi.bar)}>Сходить в бар</button>
          <button className="btn" disabled={busy} onClick={() => act(ValeraApi.badCompany)}>Выпить с маргинальными личностями</button>
          <button className="btn" disabled={busy} onClick={() => act(ValeraApi.subwaySing)}>Петь в метро</button>
          <button className="btn" disabled={busy} onClick={() => act(ValeraApi.sleep)} style={{marginLeft:'auto'}}>Спать</button>
        </div>
      </div>
    </div>
  );
}
