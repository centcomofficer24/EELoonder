using Content.Shared.Body.Organ;
using Content.Shared.Body.Events;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Timing;
using Robust.Shared.Prototypes;
namespace Content.Server.DelayedDeath;

public partial class DelayedDeathSystem : EntitySystem
{
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        using var query = EntityQueryEnumerator<DelayedDeathComponent>();
        while (query.MoveNext(out var ent, out var component))
        {
            component.DeathTimer += frameTime;

            if (component.DeathTimer >= component.DeathTime && !_mobState.IsDead(ent))
            {
                var damage = new DamageSpecifier(_prototypes.Index<DamageTypePrototype>("Bloodloss"), 155);
                _damageable.TryChangeDamage(ent, damage, partMultiplier: 0f);
            }
        }
    }
}
