namespace VsR {
	public interface IWeaponPart {
		Weapon Weapon { get; set; }

		public static void Validate(IWeaponPart weaponPart) {
			if (!weaponPart.Weapon)
				throw new System.Exception("an IWeaponPart is missing the reference to Weapon!");
		}
	}
}