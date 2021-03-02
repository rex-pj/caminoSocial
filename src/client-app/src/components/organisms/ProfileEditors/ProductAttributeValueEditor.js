import React from "react";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import { ButtonOutlineDanger } from "../../atoms/Buttons/OutlineButtons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default (props) => {
  const { attributeValue } = props;

  const onRemoveAttributeValue = (currentAttr) => {
    props.onRemoveAttributeValue(currentAttr);
  };

  const handleInputChange = (evt) => {
    let data = attributeValue;
    const { name, value } = evt.target;
    data[name] = value;
    props.onAttributeValueChange(data);
  };

  return (
    <div className="row mb-2">
      <div className="col-1"></div>
      <div className="col-12 col-md-6 col-lg-4 ps-0">
        <PrimaryTextbox
          name="name"
          value={attributeValue.name}
          placeholder="Name"
          onChange={handleInputChange}
        />
      </div>
      <div className="col-6 col-md-3 col-lg-2">
        <PrimaryTextbox
          name="priceAdjustment"
          placeholder="Price adjustment"
          onChange={handleInputChange}
          value={
            attributeValue.priceAdjustment ? attributeValue.priceAdjustment : ""
          }
        />
      </div>
      <div className="col-6 col-md-3 col-lg-2">
        <PrimaryTextbox
          name="pricePercentageAdjustment"
          placeholder="Price percentage adjustment"
          onChange={handleInputChange}
          value={
            attributeValue.pricePercentageAdjustment
              ? attributeValue.pricePercentageAdjustment
              : ""
          }
        />
      </div>
      <div className="col-12 col-md-3 col-lg-2">
        <PrimaryTextbox
          name="displayOrder"
          placeholder="Display Order"
          onChange={handleInputChange}
          value={attributeValue.displayOrder ? attributeValue.displayOrder : ""}
        />
      </div>
      <div className="col-auto">
        <ButtonOutlineDanger
          type="button"
          size="xs"
          title="Remove this attribute value"
          onClick={() => onRemoveAttributeValue(attributeValue)}
        >
          <FontAwesomeIcon icon="times"></FontAwesomeIcon>
        </ButtonOutlineDanger>
      </div>
    </div>
  );
};
