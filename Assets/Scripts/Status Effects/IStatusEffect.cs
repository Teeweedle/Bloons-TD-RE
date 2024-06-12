

public interface IStatusEffect 
{    
    void Apply(BaseBloon aBloon, BaseTower aParentTower);
    void Update(BaseBloon aBloon);
    void Remove(BaseBloon aBloon);
}
