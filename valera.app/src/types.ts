export type ValeraDto = {
    id: string;
    health: number;
    mana: number;
    vitality: number;
    tired: number;
    money: number;
    succeeded?: boolean | null;
};

export type CreateValeraRequest = {
    health?: number;
    mana?: number;
    vitality?: number;
    tired?: number;
    money?: number;
};
