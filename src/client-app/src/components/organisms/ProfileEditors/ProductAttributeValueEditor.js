import React from "react";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import { LabelPrimary } from "../../atoms/Labels";
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
      <div className="col-12 col-md-6 col-lg-4">
        <div className="row">
          <div className="col-2 mt-1">
            <LabelPrimary className="d-none d-sm-inline">Value</LabelPrimary>
          </div>
          <div className="col-10">
            <PrimaryTextbox
              name="name"
              value={attributeValue.name}
              placeholder="Name"
              onChange={handleInputChange}
            />
          </div>
        </div>
      </div>
      <div className="col-12 col-md-3 col-lg-4">
        <PrimaryTextbox
          name="priceAdjustment"
          placeholder="Price adjustment"
          onChange={handleInputChange}
          value={
            attributeValue.priceAdjustment ? attributeValue.priceAdjustment : ""
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
