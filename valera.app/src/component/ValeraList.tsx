/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from "react";
import { ValeraApi } from "../api";
import type { ValeraDto, CreateValeraRequest } from "../types";

export function ValeraList({
    onOpen,
}: {
    onOpen: (item: ValeraDto) => void;
}) {
    const [items, setItems] = useState<ValeraDto[]>([]);
    const [formOpen, setFormOpen] = useState(false);
    const [form, setForm] = useState<CreateValeraRequest>({  });
    const [loading, setLoading] = useState(false);
    const [err, setErr] = useState<string | null>(null);

    const load = async () => {
        setErr(null);
        try {
            setItems(await ValeraApi.list());
        } catch (e: any) {
            setErr(e.message ?? "Ошибка загрузки");
        }
    };

    useEffect(() => { load(); }, []);

    const onCreate = async () => {
        setLoading(true);
        setErr(null);
        try {
            await ValeraApi.create(form);
            setFormOpen(false);
            await load();
        } catch (e: any) {
            setErr(e.message ?? "Ошибка создания");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="vstack">
            <div className="hstack">
                <button className="btn primary" onClick={() => setFormOpen(true)}>Создать Валеру</button>
                <button className="btn" onClick={load}>Обновить</button>
                {err && <span className="badge" style={{ borderColor: '#ff9b9b', background: '#fff5f5' }}>⚠ {err}</span>}
            </div>

            {formOpen && (
                <div className="card vstack">
                    <div className="hstack" style={{ justifyContent: 'space-between' }}>
                        <h3 className="title" style={{ fontSize: 18, margin: 0 }}>Новая Валера</h3>
                        <button className="btn" onClick={() => setFormOpen(false)}>Закрыть</button>
                    </div>
                    <div className="hstack">
                        <input className="input" type="number" placeholder="Health" value={form.health ?? ""} onChange={e => setForm({ ...form, health: +e.target.value })} />
                        <input className="input" type="number" placeholder="Mana" value={form.mana ?? ""} onChange={e => setForm({ ...form, mana: +e.target.value })} />
                    </div>
                    <div className="hstack">
                        <input className="input" type="number" placeholder="Vitality" value={form.vitality ?? ""} onChange={e => setForm({ ...form, vitality: +e.target.value })} />
                        <input className="input" type="number" placeholder="Tired" value={form.tired ?? ""} onChange={e => setForm({ ...form, tired: +e.target.value })} />
                        <input className="input" type="number" placeholder="Money" value={form.money ?? ""} onChange={e => setForm({ ...form, money: +e.target.value })} />
                    </div>
                    <div className="hstack">
                        <button className="btn primary" disabled={loading} onClick={onCreate}>Создать</button>
                        <button className="btn" onClick={() => setFormOpen(false)}>Отмена</button>
                    </div>
                </div>
            )}

            <div className="grid">
                {items.map(v => (
                    <div className="card vstack" key={v.id}>
                        <p className="subtitle">Здоровье: {v.health} • Мана: {v.mana} • Усталость: {v.tired}</p>
                        <p className="subtitle">Жизнерадостность: {v.vitality} • Деньги: {v.money}</p>
                        <button className="btn" onClick={async () => onOpen(await ValeraApi.get(v.id))}>Открыть</button>
                    </div>
                ))}
            </div>
        </div>
    );
}